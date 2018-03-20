using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Se {
	
		/*AsyncOperation async;

		// Use this for initialization
		public void LoadingScreeenManager (int scene) {
			StartCoroutine (LoadingScreen (scene));
		}
		
		IEnumerator LoadingScreen(int scene) {
			LoadingScreenObj.SetActive (true);
			async = SceneManager.LoadSceneAsync (scene);
			async.allowSceneActivation = false;
			while (async.isDone == false) {
				ImageRotate.transform.Rotate (0, Time.deltaTime * 10, 0);
				if (async.progress == 0.9f) {
					async.allowSceneActivation = true;
				}
				yield return null;
			}
		}*/

	public class LoadingScreenManager : MonoBehaviour {

		public GameObject LoadingScreenObj;
		public GameObject ImageRotate;

		private static GameObject instance;
		public static LoadingScreenManager GetInstance() {
			if (!instance)
				instance = GameObject.FindGameObjectWithTag ("LoadingScreenManager");
			return instance.GetComponent<LoadingScreenManager>();
		}

		public void Start() {
			LoadingScreenObj.SetActive (false);
		}

		public void Show(float seconds) {
			StartCoroutine (ShowLoadingScreen (Time.time, seconds));
		}

		IEnumerator ShowLoadingScreen(float startTime, float seconds) {
			LoadingScreenObj.SetActive (true);
			while (startTime + seconds > Time.time) {
				ImageRotate.transform.Rotate (0, 0, Time.deltaTime * 100);
				yield return null;
			}
			LoadingScreenObj.SetActive (false);
		}
	}
}