using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Se {

	public class LoadingScreenManager : MonoBehaviour {

		public GameObject LoadingScreenObj;
		public GameObject ImageRotate;

		AsyncOperation async;

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
		}
	}
}