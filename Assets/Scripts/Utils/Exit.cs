namespace Utils {
	/// <summary>
	/// A convenience static class for exitting the game quickly, both in the Editor and the final game.
	/// </summary>
	public static class Exit {
		// NOTE: I would have named this method Exit() instead, but C# won't allow me.
		public static void Now() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			UnityEngine.Application.Quit ();
		}
		public static void If(bool doIt, string reason="") {
			if (doIt) {
				if (!string.IsNullOrEmpty (reason)) {
					// NOTE: Don't throw an exception, otherwise we won't reach the Exit.Now() line.
					UnityEngine.Debug.LogError("Exitting: " + reason);
				}
				Exit.Now ();
			}
		}
	}
}