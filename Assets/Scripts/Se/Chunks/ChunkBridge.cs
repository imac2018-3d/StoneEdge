using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {

    [RequireComponent(typeof(Collider))]
    public class ChunkBridge : MonoBehaviour {
        public enum SyncOrAsync {
            Sync, Async,
        };

        public SyncOrAsync Mode;
        public ChunkID[] ChunksToLoad;

        public bool IsSync { get { return Mode == SyncOrAsync.Sync; } }
        public bool IsAsync { get { return Mode == SyncOrAsync.Async; } }

        void Start() {
            Assert.IsNotNull (ChunksToLoad);
            GetComponent<Collider> ().isTrigger = true;
            gameObject.isStatic = true;
        }

        void OnTriggerEnter(Collider cld) {
            var ce = cld.gameObject.GetComponent<ChunkEnterer> ();
            if (ce == null)
                return;
            Debug.Log ("\"" + ce.gameObject.name + "\" entered \"" + gameObject.name + "\"");
            Chunks.RegisterBridge (this);
        }
        void OnTriggerExit(Collider cld) {
            var ce = cld.gameObject.GetComponent<ChunkEnterer> ();
            if (ce == null)
                return;
            Debug.Log ("\"" + ce.gameObject.name + "\" left \"" + gameObject.name + "\"");
            Chunks.UnregisterBridge (this);
        }
    }

}
