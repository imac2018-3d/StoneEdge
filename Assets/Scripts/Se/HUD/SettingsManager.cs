using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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
		public InputField JumpXbox;
		public InputField DodgeXbox;
		public InputField BasicAttackXbox;
		public InputField MagnetImpactXbox;
		public InputField JumpQuakeXbox;

		private List<KeyCode> reservedKeyboardInputs = new List<KeyCode> { KeyCode.Backspace, KeyCode.Return, KeyCode.Escape };
		private List<XbButton> reservedXboxControllerInputs = new List<XbButton> { XbButton.Start, XbButton.Back };
	
		private KeyCode newJump;
		private KeyCode newDodge;
		private KeyCode newBasicAttack;
		private KeyCode newMagnetImpact;
		private KeyCode newJumpQuake;
		private XbButton newJumpXbox;
		private XbButton newDodgeXbox;
		private XbButton newBasicAttackXbox;
		private XbButton newMagnetImpactXbox;
		private XbButton newJumpQuakeXbox;
		private float newAmbientVolume;
		private float newMusicVolume;
		private float newActionVolume;


		// Use this for initialization
		void Start () {
			/*var resolutions = new List<String>();
			foreach (var resolution in Screen.resolutions) {
				resolutions.Add (resolution.ToString());
			}
			ScreenResolution.AddOptions (resolutions);
			ScreenResolution.value = Array.IndexOf(Screen.resolutions, Screen.currentResolution);

			Windowed.isOn = !Screen.fullScreen;*/

			CurrentUserSettings.LoadSettings ();

			AmbientAudioVolume.minValue = 0.0f;
			AmbientAudioVolume.maxValue = 1.0f;
			AmbientAudioVolume.value = CurrentUserSettings.Data.AmbientVolume;
			newAmbientVolume = CurrentUserSettings.Data.AmbientVolume;

			ActionAudioVolume.minValue = 0.0f;
			ActionAudioVolume.maxValue = 1.0f;
			ActionAudioVolume.value = CurrentUserSettings.Data.ActionVolume;
			newActionVolume = CurrentUserSettings.Data.ActionVolume;

			MusicAudioVolume.minValue = 0.0f;
			MusicAudioVolume.maxValue = 1.0f;
			MusicAudioVolume.value = CurrentUserSettings.Data.MusicVolume;
			newMusicVolume = CurrentUserSettings.Data.MusicVolume;


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

			JumpXbox.text = CurrentUserSettings.Data.XbButtonJump;
			newJumpXbox = (XbButton)System.Enum.Parse (typeof(XbButton), JumpXbox.text);
			reservedXboxControllerInputs.Add (newJumpXbox);

			DodgeXbox.text = CurrentUserSettings.Data.XbButtonDodge;
			newDodgeXbox = (XbButton)System.Enum.Parse (typeof(XbButton), DodgeXbox.text);
			reservedXboxControllerInputs.Add (newDodgeXbox);

			BasicAttackXbox.text = CurrentUserSettings.Data.XbButtonBasicAttack;
			newBasicAttackXbox = (XbButton)System.Enum.Parse (typeof(XbButton), BasicAttackXbox.text);
			reservedXboxControllerInputs.Add (newBasicAttackXbox);

			MagnetImpactXbox.text = CurrentUserSettings.Data.XbButtonMagnetImpact;
			newMagnetImpactXbox = (XbButton)System.Enum.Parse (typeof(XbButton), MagnetImpactXbox.text);
			reservedXboxControllerInputs.Add (newMagnetImpactXbox);

			JumpQuakeXbox.text = CurrentUserSettings.Data.XbButtonJumpQuake;
			newJumpQuakeXbox = (XbButton)System.Enum.Parse (typeof(XbButton), JumpQuakeXbox.text);
			reservedXboxControllerInputs.Add (newJumpQuakeXbox);
		}
		
		// Update is called once per frame
		void Update () {
			if (InputActions.MenuConfirms) {
				CurrentUserSettings.Save ();
			}
			foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyDown (kcode)) {
					if (!reservedKeyboardInputs.Contains (kcode)) {
						if (Jump.isFocused) {
							reservedKeyboardInputs.Remove (newJump);
							newJump = kcode;
							Jump.text = newJump.ToString ();
							reservedKeyboardInputs.Add (newJump);
						}

						if (Dodge.isFocused) {
							reservedKeyboardInputs.Remove (newDodge);
							newDodge = kcode;
							Dodge.text = newDodge.ToString ();
							reservedKeyboardInputs.Add (newDodge);
						}

						if (BasicAttack.isFocused) {
							reservedKeyboardInputs.Remove (newBasicAttack);
							newBasicAttack = kcode;
							BasicAttack.text = newBasicAttack.ToString ();
							reservedKeyboardInputs.Add (newBasicAttack);
						}

						if (MagnetImpact.isFocused) {
							reservedKeyboardInputs.Remove (newMagnetImpact);
							newMagnetImpact = kcode;
							MagnetImpact.text = newMagnetImpact.ToString ();
							reservedKeyboardInputs.Add (newMagnetImpact);
						}

						if (JumpQuake.isFocused) {
							reservedKeyboardInputs.Remove (newJumpQuake);
							newJumpQuake = kcode;
							JumpQuake.text = newJumpQuake.ToString ();
							reservedKeyboardInputs.Add (newJumpQuake);
						}
					}
				}
			}

			foreach(XbButton button in Enum.GetValues(typeof(XbButton))) {
				if (XbInput.GetButtonDown(button)) {
					if (!reservedXboxControllerInputs.Contains (button)) {
						if (JumpXbox.isFocused) {
							reservedXboxControllerInputs.Remove(newJumpXbox);
							newJumpXbox = button;
							JumpXbox.text = newJumpXbox.ToString ();
							reservedXboxControllerInputs.Add (newJumpXbox);
						}

						if (DodgeXbox.isFocused) {
							reservedXboxControllerInputs.Remove(newDodgeXbox);
							newDodgeXbox = button;
							DodgeXbox.text = newDodgeXbox.ToString ();
							reservedXboxControllerInputs.Add (newDodgeXbox);
						}

						if (BasicAttackXbox.isFocused) {
							reservedXboxControllerInputs.Remove(newBasicAttackXbox);
							newBasicAttackXbox = button;
							BasicAttackXbox.text = newBasicAttackXbox.ToString ();
							reservedXboxControllerInputs.Add (newBasicAttackXbox);
						}

						if (MagnetImpactXbox.isFocused) {
							reservedXboxControllerInputs.Remove(newMagnetImpactXbox);
							newMagnetImpactXbox = button;
							MagnetImpactXbox.text = newMagnetImpactXbox.ToString ();
							reservedXboxControllerInputs.Add (newMagnetImpactXbox);
						}

						if (JumpQuakeXbox.isFocused) {
							reservedXboxControllerInputs.Remove(newJumpQuakeXbox);
							newJumpQuakeXbox = button;
							JumpQuakeXbox.text = newJumpQuakeXbox.ToString ();
							reservedXboxControllerInputs.Add (newJumpQuakeXbox);
						}
					}
				}
			}

		}

		/*public void SetResolution(int id) {
			Screen.SetResolution (Screen.resolutions[id].width, Screen.resolutions[id].height, Windowed.isOn);
		}

		public void SetWindowed(bool windowed) {
			Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, windowed);
		}*/

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

			CurrentUserSettings.SetXbButtonJump (newJumpXbox.ToString());
			CurrentUserSettings.SetXbButtonDodge (newDodgeXbox.ToString());
			CurrentUserSettings.SetXbButtonBasicAttack (newBasicAttackXbox.ToString());
			CurrentUserSettings.SetXbButtonMagnetImpact (newMagnetImpactXbox.ToString());
			CurrentUserSettings.SetXbButtonJumpQuake (newJumpQuakeXbox.ToString());

			CurrentUserSettings.Save ();
		}
	}
}