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
		public Dropdown BasicAttack;

		private List<KeyCode> posibleKeyboardInputs = new List<KeyCode> { KeyCode.Backspace,
																		KeyCode.Delete,
																		KeyCode.Tab,
																		KeyCode.Return,
																		KeyCode.Escape,
																		KeyCode.Space,
																		KeyCode.UpArrow,
																		KeyCode.DownArrow,
																		KeyCode.RightArrow,
																		KeyCode.LeftArrow,
																		KeyCode.A,
																		KeyCode.D,
																		KeyCode.E,
																		KeyCode.F,
																		KeyCode.Q,
																		KeyCode.S,
																		KeyCode.Z,
																		KeyCode.RightShift,
																		KeyCode.LeftShift,
																		KeyCode.RightAlt,
																		KeyCode.LeftAlt,
																		KeyCode.RightControl,
																		KeyCode.LeftControl
																	};

		private MenuManager menuManager;

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

			AmbientAudioVolume.value = 0.5f;
			AmbientAudioVolume.minValue = 0.0f;
			AmbientAudioVolume.maxValue = 1.0f;

			var keyboardBindings = new List<String> ();
			foreach (var binding in posibleKeyboardInputs) {
				keyboardBindings.Add (binding.ToString ());
			}

			BasicAttack.AddOptions (keyboardBindings);
			BasicAttack.value = keyboardBindings.IndexOf(InputActions.Bindings.BasicAttack.Keyboard.ToString());
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void setResolution(int id) {
			Screen.SetResolution (Screen.resolutions[id].width, Screen.resolutions[id].height, Windowed.isOn);
		}

		public void setWindowed(bool windowed) {
			Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, Windowed.isOn);
		}

		public void setAmbientVolume() {
			menuManager.AmbientSound.SetVolume(AmbientAudioVolume.value);
		}

		public void SetBasicJump(int id) {
			InputActions.Bindings.BasicAttack.Keyboard = posibleKeyboardInputs [id];
		}
	}
}