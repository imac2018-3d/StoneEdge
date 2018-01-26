using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Se {
	public static class GameState {
		public static bool IsPaused;

		public static void Pause() {
			foreach (GameObject go in Object.FindObjectsOfType(typeof(GameObject)) as GameObject[])
	        {
	        	go.Freeze();
	        }
			IsPaused = true;
		}

		public static void Resume() {
			IsPaused = false;
		}

	}
}
