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

		public void Start() {
			Se.GameState.IsPaused = true;
			ShowMenu (MainMenu);
			gameStarted = false;
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
			if (isNew) { CurrentGameSaveData.NewGame (); }
			else { CurrentGameSaveData.LoadGame(); }
			GameState.Resume ();
		}
			
		public void ExitGame() {
			Application.Quit ();
		}

		// NOTE: Returns to game and hide all menus
		public void ResumeGame() {
			lastMenu = null;
			Se.GameState.Resume ();
			foreach (GameObject menu in Menus) {
				menu.SetActive (false);
			}
		}

		// NOTE: Pause the game and shows Pause Menu
		public void PauseGame() {
			lastMenu = null;
			EventSystem.current.SetSelectedGameObject(PauseMenu.transform.GetChild(0).gameObject);
			Se.GameState.Pause ();
			ShowMenu (PauseMenu);
		}
	}

}