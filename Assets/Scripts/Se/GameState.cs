using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Se {
	public static class GameState {

		// NOTE: Pausing the game and freezing the world are conceptually separate.
		// Pausing the game conceptually means "entering pause menu" and requires freezing the world.
		//
		// But pausing the game is not the only reason to freeze the world : For instance, when entering
		// a Sync ChunkBridge and waiting for chunks to load, we want to freeze everything until the chunks are loaded.
		// In the same way, when playing cinematics, we'll want to freeze parts of the world too, but it's not _pausing_ the game.

		// These ones are private so nobody else can set them.
		// The only way to set them is to explicitly
		// call Pause/Resume and FreezeTheWorld/DefreezeTheWorld, which is what we want.
		public static bool IsPaused { get; private set; }
		// This one is a count of how many "systems" in the game "have a hold" of freezing the world.
		// For instance, both the Pause Menu system and the Chunks system may want to freeze the world,
		// and we don't want to defreeze the world until both systems "agree".
		// IsWorldFrozen is incremented by 1 every time FreezeTheWorld() is called.
		// IsWorldFrozen is decremented by 1 every time DefreezeTheWorld() is called.
		// The world is frozen only as long as (IsWorldFrozen > 0).
		public static int IsWorldFrozen { get; private set; }
	
		static HashSet<GameObject> lastFrozenObjects = new HashSet<GameObject>();

		public static HashSet<GameObject> FindObjectsToFreeze() {
			var h = new HashSet<GameObject> (); // HashSet ensures there are no duplicates.
			foreach (var i in GameObject.FindObjectsOfType<Camera>())
				h.Add (i.gameObject);
			foreach (var i in GameObject.FindObjectsOfType<Hero>())
				h.Add (i.gameObject);
			foreach (var i in GameObject.FindObjectsOfType<Enemy>())
				h.Add (i.gameObject);
			Debug.Log ("Found "+h.Count+" GameObjects to freeze");
			return h;
		}

		public static void FreezeTheWorld() {
			++IsWorldFrozen;
			Debug.Log ("IsWorldFrozen is now " + IsWorldFrozen);
			if (IsWorldFrozen > 1)
				return;
			Debug.Log ("Freezing the world now!");
			lastFrozenObjects = FindObjectsToFreeze ();
			foreach(var go in lastFrozenObjects) {
	        	go.Freeze();
	        }
		}
		public static void DefreezeTheWorld() {
			--IsWorldFrozen;
			if (IsWorldFrozen < 0)
				IsWorldFrozen = 0;
			Debug.Log ("IsWorldFrozen is now " + IsWorldFrozen);
			if (IsWorldFrozen > 0)
				return;
			Debug.Log ("Defreezing the world now!");
			foreach(var go in lastFrozenObjects) {
	        	go.Defreeze();
	        }
		}

		public static void Pause() {
			if (IsPaused)
				return;
			Debug.Log ("Now pausing the game");
			IsPaused = true;
			FreezeTheWorld ();
		}

		public static void Resume() {
			if (!IsPaused)
				return;
			Debug.Log ("Now resuming the game");
			IsPaused = false;
			DefreezeTheWorld ();
		}
	}
}
