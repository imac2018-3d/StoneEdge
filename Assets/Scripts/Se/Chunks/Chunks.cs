/// <summary>
/// The unique Chunks Manager.
/// It manages the loading and unloading of chunks.
/// ChunkBridges only give hints to the Chunks Manager.
/// </summary>

// Known issues:
// - OnTriggerEnter doesn't work in Edit Mode with [ExecuteInEditMode].
// - ChunkBridges are not displayed in the editor.
//   Just click on the object in the hierarchy.
// - ChunkBridges have a single collider, not a collider hierarchy.
// - Chunks are unloaded when reaching a sync Bridge and they're not
//   referenced.

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

namespace Se {

    public enum ChunkID {
        Jungle = 1,
        Desert = 2,
        Sea = 3,
        Lava = 4,
    }

    public class Chunks : MonoBehaviour {
        public enum State { Unloaded, Loading, Loaded, Unloading, }

        static Dictionary<ChunkID, Info> loadInfos() {
            return new Dictionary<ChunkID, Info>() {
                { ChunkID.Jungle, new Info("Jungle") },
                { ChunkID.Desert, new Info("Desert") },
                { ChunkID.Sea, new Info("Sea") },
                { ChunkID.Lava, new Info("Lava") },
            };
        }

        public class Info {
            public State State;
            public string SceneName = null;
            public AsyncOperation AsyncLoadingOp = null;
            public AsyncOperation AsyncUnloadingOp = null;
            public HashSet<ChunkBridge> InterestedChunkBridges = new HashSet<ChunkBridge>();

            bool isLoaded { get { var s = GetSceneIfLoaded (); return s.HasValue && s.Value.isLoaded; } }

            Info() {}
            internal Info(string sceneName) {
                State = isLoaded ? State.Loaded : State.Unloaded;
                SceneName = sceneName;
                Assert.IsTrue(SceneUtility.GetBuildIndexByScenePath(SceneName) >= 0, "Scene \"" + sceneName + "\" has no build index");
            }
            public Scene? GetSceneIfLoaded() {
                var s = SceneManager.GetSceneByName(SceneName); // NOTE: Only searches in loaded scenes !
                if(s.buildIndex < 0 || !s.IsValid())
                    return null;
                return s;
            }
        }

        static Dictionary<ChunkID, Info> cachedInfos = null;

        public static Dictionary<ChunkID, Info> Infos { 
            get {
                if(cachedInfos == null) {
                    Debug.Log ("Chunks: Refreshing Infos");
                    cachedInfos = loadInfos ();
                    assertAllChunksHaveMatchingScene ();
                }
                return cachedInfos;
            }
        }

        static void assertAllChunksHaveMatchingScene() {
            bool ok = true;
            foreach (ChunkID c in Enum.GetValues(typeof(ChunkID))) {
                if (!Infos.ContainsKey (c)) {
                    ok = false;
                    Debug.LogError ("ChunkID " + c + " does NOT have a matching scene name!");
                }
            }
            if (!ok) {
                Application.Quit (); // FIXME replace by Exit.If().
            }
        }
            
        static Chunks cachedInstance = null;
        public static Chunks Instance {
            get {
                if (cachedInstance == null) {
                    var o = FindObjectsOfType<Chunks> ();
                    Assert.AreEqual(o.Length, 1, "There must be exactly one Chunks object!");
                    cachedInstance = o[0];
                }
                return cachedInstance;
            }
        }

        void Start() {
            Assert.AreEqual(this, Instance); // Get the instance once to force load it.
        }

        static AsyncOperation unloadUnusedAssetsAsyncOp = null;

        static void startUnloadingAllUnusedChunks() {
            foreach(var i in Infos.Values.Where(v => v.State == State.Loaded && v.InterestedChunkBridges.Count==0)) {
                Debug.Log ("An unloading job is started for \""+i.SceneName+"\"");
                i.AsyncUnloadingOp = SceneManager.UnloadSceneAsync (i.SceneName);
                i.State = State.Unloading;
            }
        }

        static List<ChunkID> syncWaitChunks = new List<ChunkID>();

        public static void RegisterBridge(ChunkBridge bridge) {
            Assert.IsNotNull (bridge);
            foreach(var c in bridge.ChunksToLoad) {
                var i = Infos [c];
                i.InterestedChunkBridges.Add(bridge);
                if (i.State == State.Unloaded || i.State == State.Unloading) {
                    Debug.Log ("A loading job is started for \""+i.SceneName+"\"");
                    i.AsyncLoadingOp = SceneManager.LoadSceneAsync (i.SceneName, LoadSceneMode.Additive);
                    i.State = State.Loading;
                }
            }
            if (bridge.IsSync) {
                startUnloadingAllUnusedChunks ();
                syncWaitChunks.Clear ();
                foreach (var c in bridge.ChunksToLoad.Where(c => Infos[c].State != State.Loaded)) {
                    syncWaitChunks.Add (c);
                }
                if(syncWaitChunks.Count > 0) {
                    freezeTheWorld ();
                }
            }
        }
        public static void UnregisterBridge(ChunkBridge bridge) {
            Assert.IsNotNull (bridge);
            foreach(var c in bridge.ChunksToLoad) {
                Infos[c].InterestedChunkBridges.Remove(bridge);
            }
        }

        void Update() {
            // Poll all loading chunks
            foreach (var i in Infos.Values.Where(v => v.State == State.Loading)) {
                Debug.Log ("Loading \""+i.SceneName+"\" ("+(100f*i.AsyncLoadingOp.progress)+"%)");
                if (i.AsyncLoadingOp.isDone) {
                    i.AsyncLoadingOp = null;
                    i.State = State.Loaded;
                }
            }
            // Poll all unloading chunks
            bool someWereUnloaded = false;
            foreach (var i in Infos.Values.Where(v => v.State == State.Unloading)) {
                Debug.Log ("Unloading \""+i.SceneName+"\" ("+(100f*i.AsyncUnloadingOp.progress)+"%)");
                if (i.AsyncUnloadingOp.isDone) {
                    i.AsyncUnloadingOp = null;
                    i.State = State.Unloaded;
                    someWereUnloaded = true;
                }
            }
            if (someWereUnloaded && unloadUnusedAssetsAsyncOp == null) {
                unloadUnusedAssetsAsyncOp = Resources.UnloadUnusedAssets ();
            }
            // Then some time later...
            if (unloadUnusedAssetsAsyncOp != null && unloadUnusedAssetsAsyncOp.isDone) {
                unloadUnusedAssetsAsyncOp = null;
            }
            // If all required Sync Chunks are loaded, resume the game.
            if (syncWaitChunks.Count > 0 && syncWaitChunks.All (c => Infos [c].State == State.Loaded)) {
                defreezeTheWorld ();
                syncWaitChunks.Clear();
            }
        }

        static void freezeTheWorld() {
            // FIXME freezeTheWorld()
        }
        static void defreezeTheWorld() {
            // FIXME defreezeTheWorld()
        }
    }
}
