using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Se {
	
	public class LoreItemsPage : MonoBehaviour {

		public List<GameObject> LoreButtons;
		public GameObject LoreContent;
	

		public void Start () {
			foreach(var lore in LoreItemManager.Lores) {
				int id = lore.Key;
				if (CurrentGameSaveData.Data.LoreItems != null) {
					foreach (int j in CurrentGameSaveData.Data.LoreItems.Where(d => d == id)) {
						LoreButtons[id].GetComponent<Button> ().interactable = true;
					}
					LoreButtons[id].GetComponent<Button>().onClick.AddListener(delegate{ShowImage(id);});
				}
			}
		}
			
		public void ShowImage(int id) {
			LoreContent.GetComponent<Text> ().text = LoreItemManager.Lores[id].Content;
		}
	}


}