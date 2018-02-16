using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Se {
	public class SettingsManager : MonoBehaviour {

		public Dropdown ScreenResolution;
		public Toggle Windowed;
		public Slider AmbientAudioVolume;
		public Slider MusicAudioVolume;
		public Slider ActionAudioVolume;
		public InputField Jump;
		public InputField Dodge;
		public InputField BasicAttack;
		public InputField MagnetImpact;
		public InputField JumpQuake;

		private List<KeyCode> reservedKeyboardInputs = new List<KeyCode> { KeyCode.Backspace, KeyCode.Return, KeyCode.Escape };
	
		private KeyCode newJump;
		private KeyCode newDodge;
		private KeyCode newBasicAttack;
		private KeyCode newMagnetImpact;
		private KeyCode newJumpQuake;
		private float newAmbientVolume;
		private float newMusicVolume;
		private float newActionVolume;


		// Use this for initialization
		void Start () {
			var resolutions = new List<String>();
			foreach (var resolution in Screen.resolutions) {
				resolutions.Add (resolution.ToString());
			}
			ScreenResolution.AddOptions (resolutions);
			ScreenResolution.value = Array.IndexOf(Screen.resolutions, Screen.currentResolution);

			Windowed.isOn = !Screen.fullScreen;

			CurrentUserSettings.LoadSettings (GetComponentInParent<MenuManager>());

			AmbientAudioVolume.value = CurrentUserSettings.Data.AmbientVolume;
			AmbientAudioVolume.minValue = 0.0f;
			AmbientAudioVolume.maxValue = 1.0f;
			newAmbientVolume = AmbientAudioVolume.value;

			ActionAudioVolume.value = CurrentUserSettings.Data.ActionVolume;
			ActionAudioVolume.minValue = 0.0f;
			ActionAudioVolume.maxValue = 1.0f;
			newActionVolume = ActionAudioVolume.value;

			MusicAudioVolume.value = CurrentUserSettings.Data.MusicVolume;
			MusicAudioVolume.minValue = 0.0f;
			MusicAudioVolume.maxValue = 1.0f;
			newMusicVolume = MusicAudioVolume.value;


			Jump.text = CurrentUserSettings.Data.KeyboardJump;
			newJump = (KeyCode)System.Enum.Parse (typeof(KeyCode), Jump.text);
			reservedKeyboardInputs.Add (newJump);

			Dodge.text = CurrentUserSettings.Data.KeyboardDodge;
			newDodge = (KeyCode)System.Enum.Parse (typeof(KeyCode), Dodge.text);
			reservedKeyboardInputs.Add (newDodge);

			BasicAttack.text = CurrentUserSettings.Data.KeyboardBasicAttack;
			newBasicAttack = (KeyCode)System.Enum.Parse (typeof(KeyCode), BasicAttack.text);
			reservedKeyboardInputs.Add (newBasicAttack);

			MagnetImpact.text = CurrentUserSettings.Data.KeyboardMagnetImpact;
			newMagnetImpact = (KeyCode)System.Enum.Parse (typeof(KeyCode), MagnetImpact.text);
			reservedKeyboardInputs.Add (newMagnetImpact);

			JumpQuake.text = CurrentUserSettings.Data.KeyboardJumpQuake;
			newJumpQuake = (KeyCode)System.Enum.Parse (typeof(KeyCode), JumpQuake.text);
			reservedKeyboardInputs.Add (newJumpQuake);
		}
		
		// Update is called once per frame
		void Update () {
			foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown (kcode)) {
					if (!reservedKeyboardInputs.Contains (kcode)) {
						if (Jump.isFocused) {
							reservedKeyboardInputs.Remove(newJump);
							newJump = kcode;
							Jump.text = newJump.ToString ();
							reservedKeyboardInputs.Add (newJump);
						}

						if (Dodge.isFocused) {
							reservedKeyboardInputs.Remove(newDodge);
							newDodge = kcode;
							Dodge.text = newDodge.ToString ();
							reservedKeyboardInputs.Add (newDodge);
						}

						if (BasicAttack.isFocused) {
							reservedKeyboardInputs.Remove(newBasicAttack);
							newBasicAttack = kcode;
							BasicAttack.text = newBasicAttack.ToString ();
							reservedKeyboardInputs.Add (newBasicAttack);
						}

						if (MagnetImpact.isFocused) {
							reservedKeyboardInputs.Remove(newMagnetImpact);
							newMagnetImpact = kcode;
							MagnetImpact.text = newMagnetImpact.ToString ();
							reservedKeyboardInputs.Add (newMagnetImpact);
						}

						if (JumpQuake.isFocused) {
							reservedKeyboardInputs.Remove(newJumpQuake);
							newJumpQuake = kcode;
							JumpQuake.text = newJumpQuake.ToString ();
							reservedKeyboardInputs.Add (newJumpQuake);
						}
					}
				}
			}

		}

		public void SetResolution(int id) {
			Screen.SetResolution (Screen.resolutions[id].width, Screen.resolutions[id].height, Windowed.isOn);
		}

		public void SetWindowed(bool windowed) {
			Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, Windowed.isOn);
		}

		public void SetAmbientVolume() {
			newAmbientVolume = AmbientAudioVolume.value;
		}

		public void SetActionVolume() {
			newActionVolume = ActionAudioVolume.value;
		}
			
		public void SetMusicVolume() {
			newMusicVolume = MusicAudioVolume.value;
		}

		public void SaveSettings() {
			InputActions.Bindings.BasicAttack.Keyboard = newBasicAttack;

			CurrentUserSettings.SetAmbientVolume(newAmbientVolume);
			CurrentUserSettings.SetActionVolume (newActionVolume);
			CurrentUserSettings.SetMusicVolume (newMusicVolume);

			CurrentUserSettings.SetKeyboardJump (newJump.ToString());
			CurrentUserSettings.SetKeyboardDodge (newDodge.ToString());
			CurrentUserSettings.SetKeyboardBasicAttack (newBasicAttack.ToString());
			CurrentUserSettings.SetKeyboardMagnetImpact (newMagnetImpact.ToString());
			CurrentUserSettings.SetKeyboardJumpQuake (newJumpQuake.ToString());

			CurrentUserSettings.Save ();
		}
	}
}