using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Se {
	[Serializable]
	public struct UserSettings {
		public float AmbientVolume;
		public float MusicVolume;
		public float ActionVolume;
		public String KeyboardJump;
		public String KeyboardDodge;
		public String KeyboardBasicAttack;
		public String KeyboardMagnetImpact;
		public String KeyboardJumpQuake;

		public static readonly UserSettings Empty = new UserSettings {
			AmbientVolume = 0.0f,
			MusicVolume = 0.0f,
			ActionVolume = 0.0f,
			KeyboardJump = "",
			KeyboardDodge = "",
			KeyboardBasicAttack = "",
			KeyboardMagnetImpact = "",
			KeyboardJumpQuake = "",
		};
		public bool IsEmpty {
			get { 
				return AmbientVolume == Empty.AmbientVolume
				&& MusicVolume == Empty.MusicVolume
				&& ActionVolume == Empty.ActionVolume
				&& KeyboardJump == Empty.KeyboardJump
				&& KeyboardDodge == Empty.KeyboardDodge
				&& KeyboardBasicAttack == Empty.KeyboardBasicAttack
				&& KeyboardMagnetImpact == Empty.KeyboardMagnetImpact
				&& KeyboardJumpQuake == Empty.KeyboardJumpQuake;
			}
		}
	}

	public static class CurrentUserSettings {
		public static UserSettings Data;
		public static readonly string JsonFileName = "UserSettings.json";
		public static MenuManager MenuManager;

		public static string SavePath {
			get { 
				return Path.Combine(Application.dataPath, JsonFileName);
			}
		}
			
		public static void Save() {
			File.WriteAllText(SavePath, JsonUtility.ToJson(Data));
			Debug.Log ("New basic attack" + Data.KeyboardBasicAttack);
			MenuManager.Back ();
		}

		public static void LoadSettings(MenuManager menuManager) {
			MenuManager = menuManager;
			string json = File.ReadAllText (SavePath);
			Data = JsonUtility.FromJson<UserSettings>(json);

			SetActionVolume(Data.ActionVolume);
			SetAmbientVolume(Data.AmbientVolume);
			SetMusicVolume(Data.MusicVolume);

			if (Data.KeyboardJump == "")
				SetKeyboardJump (InputActions.Bindings.Jump.Keyboard.ToString ());
			else
				SetKeyboardJump (Data.KeyboardJump);
			
			if (Data.KeyboardDodge == "")
				SetKeyboardDodge (InputActions.Bindings.Dodge.Keyboard.ToString());
			else
				SetKeyboardDodge (Data.KeyboardDodge);
			
			if (Data.KeyboardBasicAttack == "")
				SetKeyboardBasicAttack (InputActions.Bindings.BasicAttack.Keyboard.ToString());
			else
				SetKeyboardBasicAttack (Data.KeyboardBasicAttack);
			
			if (Data.KeyboardMagnetImpact == "")
				SetKeyboardMagnetImpact (InputActions.Bindings.MagnetImpact.Keyboard.ToString());
			else
				SetKeyboardMagnetImpact (Data.KeyboardMagnetImpact);
			
			if (Data.KeyboardJumpQuake == "")
				SetKeyboardJumpQuake (InputActions.Bindings.JumpQuake.Keyboard.ToString());
			else
				SetKeyboardJumpQuake (Data.KeyboardJumpQuake);

			Save ();
		}

		public static void SetAmbientVolume(float newVolume) {
			Data.AmbientVolume = newVolume;
			MenuManager.AmbientSound.SetVolume (newVolume);
		}

		public static void SetActionVolume(float newVolume) {
			Data.ActionVolume = newVolume;
			MenuManager.ActionSound.SetVolume(newVolume);
		}

		public static void SetMusicVolume(float newVolume) {
			Data.MusicVolume = newVolume;
			MenuManager.Music.SetVolume(newVolume);
		}
			
		public static void SetKeyboardJump(String newValue) {
			Data.KeyboardJump = newValue;
			InputActions.Bindings.Jump.Keyboard = (KeyCode)System.Enum.Parse (typeof(KeyCode), newValue);
		}

		public static void SetKeyboardDodge(String newValue) {
			Data.KeyboardDodge = newValue;
			InputActions.Bindings.Dodge.Keyboard = (KeyCode)System.Enum.Parse (typeof(KeyCode), newValue);
		}

		public static void SetKeyboardBasicAttack(String newValue) {
			Data.KeyboardBasicAttack = newValue;
			InputActions.Bindings.BasicAttack.Keyboard = (KeyCode)System.Enum.Parse (typeof(KeyCode), newValue);
		}

		public static void SetKeyboardMagnetImpact(String newValue) {
			Data.KeyboardMagnetImpact = newValue;
			InputActions.Bindings.MagnetImpact.Keyboard = (KeyCode)System.Enum.Parse (typeof(KeyCode), newValue);
		}

		public static void SetKeyboardJumpQuake(String newValue) {
			Data.KeyboardJumpQuake = newValue;
			InputActions.Bindings.JumpQuake.Keyboard = (KeyCode)System.Enum.Parse (typeof(KeyCode), newValue);
		}
	}
}
