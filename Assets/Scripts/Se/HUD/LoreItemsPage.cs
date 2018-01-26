using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Se {
	
	public class LoreItemsPage : MonoBehaviour {

		public GameObject LoreParent;
		public GameObject LorePrefab;
		public GameObject LoreImage;
		public List<Texture> Images;

		public void Start () {
			foreach(var lore in LoreItemManager.Lores) {
				GameObject loreGameObject = Instantiate(LorePrefab) as GameObject;
				Debug.Log (lore.Key);
				loreGameObject.transform.SetParent(LoreParent.transform);
				loreGameObject.GetComponentInChildren<Text> ().text = lore.Value.Text;
				loreGameObject.transform.localPosition = new Vector3 (loreGameObject.transform.parent.localPosition.x + 250,
																	  loreGameObject.transform.parent.localPosition.y + lore.Key*50,
																	  loreGameObject.transform.parent.localPosition.z);
				loreGameObject.transform.localScale = new Vector3(1, 1, 1);
				int id = lore.Key;
				loreGameObject.GetComponent<Button>().onClick.AddListener(delegate{ShowImage(id);});
				loreGameObject.GetComponent<Button>().interactable = false;

				foreach (int j in GameSaveData.GetLoreItems()) {
					if (j == id) {
						loreGameObject.GetComponent<Button>().interactable = true;
						break;
					}
				}
			}
		}
			
		public void ShowImage(int id) {
			var sprite = Images [id];
			LoreImage.GetComponent<RawImage> ().texture = sprite;
		}
	}


}