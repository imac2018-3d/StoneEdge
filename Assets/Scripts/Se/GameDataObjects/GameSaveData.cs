using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Se {

	[Serializable]
	public struct GameSaveData {
		public int LastCheckpoint;
		public List<int> LoreItems;
		public bool HasAcquiredMagnetImpact;
		public bool HasAcquiredJumpQuake;

		public static readonly GameSaveData Empty = new GameSaveData {
			LastCheckpoint = 0,
			LoreItems = null,
			HasAcquiredMagnetImpact = false,
			HasAcquiredJumpQuake = false,
		};
		public bool IsEmpty {
			get { 
				return LastCheckpoint == Empty.LastCheckpoint
				&& LoreItems == Empty.LoreItems
				&& HasAcquiredJumpQuake == Empty.HasAcquiredJumpQuake
				&& HasAcquiredMagnetImpact == Empty.HasAcquiredMagnetImpact;
			}
		}
	}

	public static class CurrentGameSaveData {
		public static GameSaveData Data;
		public static readonly string JsonFileName = "GameSaveData.json";
		public static bool DataChanged = false;

		public static string SavePath {
			get { 
				return Path.Combine(Application.streamingAssetsPath, JsonFileName);
			}
		}

		public static void NewGame() {
			Data = new GameSaveData ();
			Save ();
			DataChanged = true;
		}

		public static void Save() {
			File.WriteAllText(SavePath, JsonUtility.ToJson(Data));
		}

		public static void LoadGame() {
			string json = File.ReadAllText (SavePath);
			Data = JsonUtility.FromJson<GameSaveData>(json);
			DataChanged = true;
		}
	}
}
