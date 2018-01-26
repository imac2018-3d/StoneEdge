using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Se {

	public class CharacterPage : MonoBehaviour {
		public GameObject Character;
		public GameObject[] PowerUps;

		void Start () {
		}

		void Update() {
			Character.transform.Rotate (0, Time.deltaTime * 10, 0);

			for(int ability = 0; ability < PowerUps.Length; ++ ability) {
				foreach (int powerUpId in CurrentGameSaveData.Data.PowerUps) {
					if (powerUpId == ability) {
						PowerUps [ability].SetActive (true);
						break;
					}
				}
			}
		}
	}

}