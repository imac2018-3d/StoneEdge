using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Utils;

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

		public String XbButtonJump;
		public String XbButtonDodge;
		public String XbButtonBasicAttack;
		public String XbButtonMagnetImpact;
		public String XbButtonJumpQuake;

		public static readonly UserSettings Empty = new UserSettings {
			AmbientVolume = 0.5f,
			MusicVolume = 0.5f,
			ActionVolume = 0.5f,
			KeyboardJump = "",
			KeyboardDodge = "",
			KeyboardBasicAttack = "",
			KeyboardMagnetImpact = "",
			KeyboardJumpQuake = "",
			XbButtonJump = "",
			XbButtonDodge = "",
			XbButtonBasicAttack = "",
			XbButtonMagnetImpact = "",
			XbButtonJumpQuake = "",
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
				&& KeyboardJumpQuake == Empty.KeyboardJumpQuake
				&& XbButtonJump == Empty.XbButtonJump
				&& XbButtonDodge == Empty.XbButtonDodge
				&& XbButtonBasicAttack == Empty.XbButtonBasicAttack
				&& XbButtonMagnetImpact == Empty.XbButtonMagnetImpact
				&& XbButtonJumpQuake == Empty.XbButtonJumpQuake;
			}
		}
	}

	public static class CurrentUserSettings {
		public static UserSettings Data;
		public static readonly string JsonFileName = "UserSettings.json";
		public static MenuManager MenuManager;
		public static AudioManager AudioManager;

		public static string SavePath {
			get { 
				return Path.Combine(Application.streamingAssetsPath, JsonFileName);
			}
		}
			
		public static void Save() {
			File.WriteAllText(SavePath, JsonUtility.ToJson(Data));
			MenuManager.Back ();
		}

		public static void LoadSettings() {
			MenuManager = MenuManager.GetInstance();
			string json = File.ReadAllText (SavePath);
			if (json == "")
				Data = new UserSettings ();
			else
				Data = JsonUtility.FromJson<UserSettings>(json);

			AudioManager = AudioManager.GetInstance();

			SetActionVolume(Data.ActionVolume);
			SetAmbientVolume(Data.AmbientVolume);
			SetMusicVolume(Data.MusicVolume);

			if (Data.KeyboardJump == "" || Data.KeyboardJump == null || Data.KeyboardJump == "None")
				SetKeyboardJump (InputActions.Bindings.Jump.Keyboard.ToString ());
			else
				SetKeyboardJump (Data.KeyboardJump);
			
			if (Data.KeyboardDodge == "" || Data.KeyboardDodge == null || Data.KeyboardDodge == "None")
				SetKeyboardDodge (InputActions.Bindings.Dodge.Keyboard.ToString());
			else
				SetKeyboardDodge (Data.KeyboardDodge);
			
			if (Data.KeyboardBasicAttack == "" || Data.KeyboardBasicAttack == null || Data.KeyboardBasicAttack == "None")
				SetKeyboardBasicAttack (InputActions.Bindings.BasicAttack.Keyboard.ToString());
			else
				SetKeyboardBasicAttack (Data.KeyboardBasicAttack);
			
			if (Data.KeyboardMagnetImpact == "" || Data.KeyboardMagnetImpact == null || Data.KeyboardMagnetImpact == "None")
				SetKeyboardMagnetImpact (InputActions.Bindings.MagnetImpact.Keyboard.ToString());
			else
				SetKeyboardMagnetImpact (Data.KeyboardMagnetImpact);
			
			if (Data.KeyboardJumpQuake == "" || Data.KeyboardJumpQuake == null || Data.KeyboardJumpQuake == "None")
				SetKeyboardJumpQuake (InputActions.Bindings.JumpQuake.Keyboard.ToString());
			else
				SetKeyboardJumpQuake (Data.KeyboardJumpQuake);

			if (Data.XbButtonJump == "" || Data.XbButtonJump == null || Data.XbButtonJump == "None")
				SetXbButtonJump (InputActions.Bindings.Jump.XboxController.ToString ());
			else
				SetXbButtonJump (Data.XbButtonJump);
			
			if (Data.XbButtonDodge == "" || Data.XbButtonDodge == null || Data.XbButtonDodge == "None")
				SetXbButtonDodge (InputActions.Bindings.Dodge.XboxController.ToString());
			else
				SetXbButtonDodge (Data.XbButtonDodge);
			
			if (Data.XbButtonBasicAttack == "" || Data.XbButtonBasicAttack == null || Data.XbButtonBasicAttack == "None")
				SetXbButtonBasicAttack (InputActions.Bindings.BasicAttack.XboxController.ToString());
			else
				SetXbButtonBasicAttack (Data.XbButtonBasicAttack);
			
			if (Data.XbButtonMagnetImpact == "" || Data.XbButtonMagnetImpact == null || Data.XbButtonMagnetImpact == "None")
				SetXbButtonMagnetImpact (InputActions.Bindings.MagnetImpact.XboxController.ToString());
			else
				SetXbButtonMagnetImpact (Data.XbButtonMagnetImpact);
			
			if (Data.XbButtonJumpQuake == "" || Data.XbButtonJumpQuake == null || Data.XbButtonJumpQuake == "None")
				SetXbButtonJumpQuake (InputActions.Bindings.JumpQuake.XboxController.ToString());
			else
				SetXbButtonJumpQuake (Data.XbButtonJumpQuake);
	
			Save ();
		}

		public static void SetAmbientVolume(float newVolume) {
			Data.AmbientVolume = newVolume;
			AudioManager.AmbientSound.SetVolume (newVolume);
		}

		public static void SetActionVolume(float newVolume) {
			Data.ActionVolume = newVolume;
			AudioManager.ActionSound.SetVolume(newVolume);
		}

		public static void SetMusicVolume(float newVolume) {
			Data.MusicVolume = newVolume;
			AudioManager.MusicSound.SetVolume(newVolume);
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

		public static void SetXbButtonJump(String newValue) {
			Data.XbButtonJump = newValue;
			InputActions.Bindings.Jump.XboxController = (XbButton)System.Enum.Parse (typeof(XbButton), newValue);
		}

		public static void SetXbButtonDodge(String newValue) {
			Data.XbButtonDodge = newValue;
			InputActions.Bindings.Dodge.XboxController = (XbButton)System.Enum.Parse (typeof(XbButton), newValue);
		}

		public static void SetXbButtonBasicAttack(String newValue) {
			Data.XbButtonBasicAttack = newValue;
			InputActions.Bindings.BasicAttack.XboxController = (XbButton)System.Enum.Parse (typeof(XbButton), newValue);
		}

		public static void SetXbButtonMagnetImpact(String newValue) {
			Data.XbButtonMagnetImpact = newValue;
			InputActions.Bindings.MagnetImpact.XboxController = (XbButton)System.Enum.Parse (typeof(XbButton), newValue);
		}

		public static void SetXbButtonJumpQuake(String newValue) {
			Data.XbButtonJumpQuake = newValue;
			InputActions.Bindings.JumpQuake.XboxController = (XbButton)System.Enum.Parse (typeof(XbButton), newValue);
		}
	}
}
