using UnityEngine;
using System.Collections;
using System.IO;


namespace Se {

	[System.Serializable]
	public class GameSaveData {

		public int LastCheckpoint = 0;
		public int[] LoreItems = new int[0];
		public int[] PowerUps = new int[0];

		private static GameSaveData _instance;
		public static GameSaveData Instance {
			get {
				if (_instance == null) {
					Se.GameSaveData.LoadGame();
				}	
				return _instance;
			}
		}

		// NOTE: Saves datas in the gameDatas json file
		public static void Save() {
			string json =  Application.dataPath + "/gameDatas.json";
			string str = JsonUtility.ToJson(_instance);
			File.WriteAllText(json, str);
		}

		// NOTE: Load datas from the gameDatas json file
		public static void LoadGame() {
			string jsn = File.ReadAllText (Application.dataPath + "/gameDatas.json");
			_instance = JsonUtility.FromJson<GameSaveData>(jsn);
			if (Se.GameSaveData.IsEmpty()) { NewGame (); }
		}

		// NOTE: Checks if the game is new or an saved game
		public static bool IsEmpty() {
			if (Instance.LastCheckpoint == 0 && Instance.LoreItems == null && Instance.PowerUps == null) { return true; }
			else { return false; }
		}

		// NOTE: Creates a new instance of GameSaveData and save new values
		public static void NewGame() {
			_instance = new GameSaveData();
			Se.GameSaveData.Save();
		}

		public static int[] GetLoreItems() {
			return Instance.LoreItems;
		}

		public static int[] GetPowerUps() {
			return Instance.PowerUps;
		}

		public static int GetLastCheckpoint() {
			return Instance.LastCheckpoint;
		}
	}
}