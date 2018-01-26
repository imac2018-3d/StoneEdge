using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Se {

	[Serializable]
	public struct GameSaveData {
		public int LastCheckpoint;
		public List<int> LoreItems;
		public List<int> PowerUps;

		public bool IsEmpty {
			get {
				return LastCheckpoint == 0 && LoreItems == null && PowerUps == null;
			}
		}
	}

	public static class CurrentGameSaveData {
		public static GameSaveData Data;
		public static readonly string JsonFileName = "GameData.json";

		public static string SavePath {
			get { 
				return Path.Combine(Application.dataPath, JsonFileName);
			}
		}

		public static void NewGame() {
			Data = new GameSaveData ();
			Save ();
		}

		public static void Save() {
			File.WriteAllText(SavePath, JsonUtility.ToJson(Data));
		}

		public static void LoadGame() {
			string json = File.ReadAllText (SavePath);
			Data = JsonUtility.FromJson<GameSaveData>(json);
		}

		public static bool IsEmpty {
			get {
				return Data.IsEmpty;
			}
		}
	}
}