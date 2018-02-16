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
		private MenuManager menuManager;
	
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
			menuManager = GetComponentInParent<MenuManager> ();
			var resolutions = new List<String>();
			foreach (var resolution in Screen.resolutions) {
				resolutions.Add (resolution.ToString());
			}
			ScreenResolution.AddOptions (resolutions);
			ScreenResolution.value = Array.IndexOf(Screen.resolutions, Screen.currentResolution);

			Windowed.isOn = !Screen.fullScreen;

			AmbientAudioVolume.value = menuManager.AmbientSound.volume;
			AmbientAudioVolume.minValue = 0.0f;
			AmbientAudioVolume.maxValue = 1.0f;
			newAmbientVolume = menuManager.AmbientSound.volume;

			ActionAudioVolume.value = menuManager.ActionSound.volume;
			ActionAudioVolume.minValue = 0.0f;
			ActionAudioVolume.maxValue = 1.0f;
			newActionVolume = menuManager.ActionSound.volume;

			MusicAudioVolume.value = menuManager.Music.volume;
			MusicAudioVolume.minValue = 0.0f;
			MusicAudioVolume.maxValue = 1.0f;
			newMusicVolume = menuManager.Music.volume;


			Jump.text = InputActions.Bindings.Jump.Keyboard.ToString();
			newJump = InputActions.Bindings.Jump.Keyboard;
			reservedKeyboardInputs.Add (newJump);

			Dodge.text = InputActions.Bindings.Dodge.Keyboard.ToString();
			newDodge = InputActions.Bindings.Dodge.Keyboard;
			reservedKeyboardInputs.Add (newDodge);

			BasicAttack.text = InputActions.Bindings.BasicAttack.Keyboard.ToString();
			newBasicAttack = InputActions.Bindings.BasicAttack.Keyboard;
			reservedKeyboardInputs.Add (newBasicAttack);

			MagnetImpact.text = InputActions.Bindings.MagnetImpact.Keyboard.ToString();
			newMagnetImpact = InputActions.Bindings.MagnetImpact.Keyboard;
			reservedKeyboardInputs.Add (newMagnetImpact);

			JumpQuake.text = InputActions.Bindings.JumpQuake.Keyboard.ToString();
			newJumpQuake = InputActions.Bindings.JumpQuake.Keyboard;
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

			menuManager.AmbientSound.SetVolume(newAmbientVolume);
			menuManager.ActionSound.SetVolume(newActionVolume);
			menuManager.Music.SetVolume(newMusicVolume);
		}
	}
}