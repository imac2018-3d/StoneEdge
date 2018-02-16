using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Se {
	
	public class LoreItemsPage : MonoBehaviour {

		public List<GameObject> LoreButtons;
		public GameObject LoreImage;
		public List<Texture> Images;
	

		public void Start () {
			foreach(var lore in LoreItemManager.Lores) {
				/*
				GameObject loreGameObject = Instantiate(LorePrefab) as GameObject;
				int id = lore.Key;
				loreGameObject.transform.SetParent(LoreParent.transform);
				loreGameObject.GetComponentInChildren<Text> ().text = lore.Value.Text;
				Vector3 newPosition = new Vector3 (loreGameObject.transform.parent.localPosition.x + 250,
												   loreGameObject.transform.parent.localPosition.y + id*50,
												   loreGameObject.transform.parent.localPosition.z);
				loreGameObject.transform.localPosition = newPosition;
				Debug.Log (loreGameObject.transform.localPosition);
				loreGameObject.transform.localScale = new Vector3(1, 1, 1);
				loreGameObject.GetComponent<Button>().onClick.AddListener(delegate{ShowImage(id);});
				loreGameObject.GetComponent<Button>().interactable = false;
				*/
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
			var sprite = Images [id];
			LoreImage.GetComponent<RawImage> ().texture = sprite;
		}
	}


}