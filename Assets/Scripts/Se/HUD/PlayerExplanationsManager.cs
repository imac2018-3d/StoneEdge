using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Se {

	public class PlayerExplanationsManager : MonoBehaviour {
		public GameObject MagnetImpact;
		public GameObject JumpQuake;

		private static GameObject instance;
	
		public void Start() {
			HideMagnetImpact ();
			HideJumpQuake ();
		}

		public void ShowMagnetImpact() {
			HideJumpQuake ();
			MagnetImpact.GetComponentInChildren<Text> ().text = "Now you can use a new power: the Magnet Impact! To test it, click on the " + InputActions.Bindings.MagnetImpact.Keyboard.ToString() + " key or " + InputActions.Bindings.MagnetImpact.XboxController.ToString () + " XBox button!";
			MagnetImpact.SetActive (true);
		}

		public void ShowJumpQuake() {
			HideMagnetImpact ();
			JumpQuake.GetComponentInChildren<Text> ().text = "Now you can use a new power: the Jump Quake! To test it, click on the " + InputActions.Bindings.JumpQuake.Keyboard.ToString() + " key or " + InputActions.Bindings.JumpQuake.XboxController.ToString () + " XBox button!";
			JumpQuake.SetActive (true);
		}

		public void HideMagnetImpact() {
			MagnetImpact.SetActive (false);
		}

		public void HideJumpQuake() {
			JumpQuake.SetActive (false);
		}

		public static PlayerExplanationsManager GetInstance() {
			if (!instance)
				instance = GameObject.FindGameObjectWithTag ("PlayerExplanationsManager");
			return instance.GetComponent<PlayerExplanationsManager>();
		}
	}

}