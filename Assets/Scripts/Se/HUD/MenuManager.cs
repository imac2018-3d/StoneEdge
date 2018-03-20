using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Se {

	// NOTE: The class manages all navigation between menus and when pause / resume a game.
	public class MenuManager : MonoBehaviour {
		public GameObject[] Menus;
		public GameObject MainMenu;
		public GameObject PauseMenu;

		private GameObject currentMenu;
		private GameObject lastMenu;
		private bool gameStarted;

		private AudioManager audioManager;
		private static GameObject instance;

		public void Start() {
			GameState.Pause ();
			ShowMenu (MainMenu);
			gameStarted = false;

			audioManager = AudioManager.GetInstance ();
			audioManager.PlayMusic (AudioManager.Music.Menu);
		}

		public void Update() {
			if (gameStarted) {
				if(!Se.GameState.IsPaused && InputActions.Pauses) { PauseGame (); }
				else if (Se.GameState.IsPaused && InputActions.Resumes) { ResumeGame ();	}
			}

			if (InputActions.MenuBack && lastMenu) {
				Back ();
			}
		}

		// NOTE: Shows a menu and hides all the others
		public void ShowMenu(GameObject menuToShow) {
			lastMenu = currentMenu;

			currentMenu = menuToShow;
			foreach (GameObject menu in Menus) {
				if (menu == menuToShow) { menu.SetActive(true); }
				else { menu.SetActive (false); }
			}
		}

		// NOTE: Returns to the last menu
		public void Back() {
			ShowMenu (lastMenu);
		}

		// NOTE: Loads the game
		// If "isNew" it loads a new game, else it loads last game saved
		public void LoadGame(bool isNew) {
			gameStarted = true;
			ResumeGame ();
			if (isNew) { 
				CurrentGameSaveData.NewGame ();
			}
			else { 
				CurrentGameSaveData.LoadGame();
			}
			GameState.Resume ();

		}
			
		public void ExitGame() {
			LoadingScreenManager.GetInstance ().Show (2);
			Application.Quit ();
		}

		// NOTE: Returns to game and hide all menus
		public void ResumeGame() {
			lastMenu = null;
			Se.GameState.Resume ();
			foreach (GameObject menu in Menus) {
				menu.SetActive (false);
			}
			LoadingScreenManager.GetInstance ().Show (0.5f);
			audioManager.PauseMusic ();
			audioManager.PlayAmbient ();
		}

		// NOTE: Pause the game and shows Pause Menu
		public void PauseGame() {
			lastMenu = null;
			EventSystem.current.SetSelectedGameObject(PauseMenu.transform.GetChild(0).gameObject);
			Se.GameState.Pause ();
			ShowMenu (PauseMenu);
		
			LoadingScreenManager.GetInstance ().Show (0.5f);
			audioManager.PlayMusic (AudioManager.Music.Menu);
			audioManager.PauseAmbient ();
		}

		public static MenuManager GetInstance() {
			if (!instance)
				instance = GameObject.FindGameObjectWithTag ("MenuManager");
			return instance.GetComponent<MenuManager>();
		}
	}

}