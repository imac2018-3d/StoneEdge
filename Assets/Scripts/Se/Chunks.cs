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

#define DEADLINE_APPROACHING

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using Utils;

namespace Se {

	// IT IS NOT FINE to change the value of, or remove, a true chunk of the game, because it would break everything.
	// 
	// Therefore, please only add new chunks, with a unique value, 
	// and DO NOT remove any existing one. Only mark them as obsolete in the loadInfos() method.
    public enum ChunkID {
		TempleForest = 5,
		Forest = 6,
		Nexus = 7,
		Desert = 8,
		DescenteDesert = 9,
		TempleDesert = 10,
		Canyon = 11,
		Bridge = 12,
		Plain = 13,
		Village = 14,
		TempleVillage = 15,
		Mountain = 16,

		Test0 = 100,
		Test1 = 101,
		Test2 = 102,
		Test3 = 103,
    }

    public class Chunks : MonoBehaviour {
        public enum State { Unloaded, Loading, Loaded, Unloading, }
		public enum Obsolete { Yes, No, }

        static Dictionary<ChunkID, Info> loadInfos() {
            return new Dictionary<ChunkID, Info>() {
				#if DEADLINE_APPROACHING
				{ ChunkID.Test0, new Info("ChunkTest0", Obsolete.No) },
				{ ChunkID.Test1, new Info("ChunkTest1", Obsolete.No) },
				{ ChunkID.Test2, new Info("ChunkTest2", Obsolete.No) },
				{ ChunkID.Test3, new Info("ChunkTest3", Obsolete.No) },
				#else
				{ ChunkID.Test0, new Info("ChunkTest0", Obsolete.Yes) },
				{ ChunkID.Test1, new Info("ChunkTest1", Obsolete.Yes) },
				{ ChunkID.Test2, new Info("ChunkTest2", Obsolete.Yes) },
				{ ChunkID.Test3, new Info("ChunkTest3", Obsolete.Yes) },
				{ ChunkID.TempleForest, new Info("TempleForest") },
				{ ChunkID.Forest, new Info("Forest") },
				{ ChunkID.Nexus, new Info("Nexus") },
				{ ChunkID.Desert, new Info("Desert") },
				{ ChunkID.DescenteDesert, new Info("DescenteDesert") },
				{ ChunkID.TempleDesert, new Info("TempleDesert") },
				{ ChunkID.Canyon, new Info("Canyon") },
				{ ChunkID.Bridge, new Info("Bridge") },
				{ ChunkID.Plain, new Info("Plain") },
				{ ChunkID.Village, new Info("Village") },
				{ ChunkID.TempleVillage, new Info("TempleVillage") },
				{ ChunkID.Mountain, new Info("Mountain") },
				#endif
            };
        }

        public class Info {
			public Obsolete Obsolete;
            public State State;
            public string SceneName = null;
            public AsyncOperation AsyncLoadingOp = null;
            public AsyncOperation AsyncUnloadingOp = null;
            public HashSet<ChunkBridge> InterestedChunkBridges = new HashSet<ChunkBridge>();

            bool isLoaded { get { var s = GetSceneIfLoaded (); return s.HasValue && s.Value.isLoaded; } }

            Info() {}
			internal Info(string sceneName, Obsolete obsolete = Obsolete.No) {
               	SceneName = sceneName;
				Obsolete = obsolete;
				switch(Obsolete) {
				case Obsolete.No:
                	State = isLoaded ? State.Loaded : State.Unloaded;
                	Assert.IsTrue(SceneUtility.GetBuildIndexByScenePath(SceneName) >= 0, "Scene \"" + sceneName + "\" has no build index");
					break;
				case Obsolete.Yes:
                	State = State.Unloaded;
					Assert.IsNotNull(AllBridges); // Fetch AllBridges at least once, which ensures no bridge refers to obsolete chunks.
					break;
				}
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
		static ChunkBridge[] cachedAllBridges = null;
		public static ChunkBridge[] AllBridges {
			get {
				if (cachedAllBridges == null) {
					cachedAllBridges = FindObjectsOfType<ChunkBridge> ();
					// Ensure that no bridge refers to obsolete chunks
					foreach(var b in cachedAllBridges) {
						foreach (var c in b.ChunksToLoad) {
							Assert.AreNotEqual (
								Infos [c].Obsolete, Obsolete.Yes, 
								"ChunkBridge \""+b.gameObject.name+"\" refers to an obsolete ChunkID ("+c+")!"
							);
						}
					}
				}
				return cachedAllBridges;
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
#if !DEADLINE_APPROACHING
			Exit.If (!ok, "All ChunkIDs must have a matching scene name!");
#endif
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
					GameState.FreezeTheWorld();
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
				Debug.Log ("Trying to defreeze the world!");
				GameState.DefreezeTheWorld();
                syncWaitChunks.Clear();
            }
        }
    }
}
