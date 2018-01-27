using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {

	public class CharacterPage : MonoBehaviour {
		public GameObject Character;
		// NOTE(yoanlcq): these two replace the PowerUp[] array.
		public GameObject AcquiredMagnetImpact;
		public GameObject AcquiredJumpQuake;

		void Start () {
			Assert.IsNotNull (Character);
			Assert.IsNotNull (AcquiredMagnetImpact);
			Assert.IsNotNull (AcquiredJumpQuake);
		}

		void Update() {
			Character.transform.Rotate (0, Time.deltaTime * 10, 0);

			if (CurrentGameSaveData.Data.HasAcquiredMagnetImpact)
				AcquiredMagnetImpact.SetActive (true);
			if (CurrentGameSaveData.Data.HasAcquiredJumpQuake)
				AcquiredJumpQuake.SetActive (true);
		}
	}
}