using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject controlMenu;

	public void Play(){
		SceneManager.LoadScene ("GameDemo");
	}

	public void MainMenu(){
		mainMenu.SetActive (true);
		controlMenu.SetActive (false);
	}

	public void ControlsMenu(){
		mainMenu.SetActive (false);
		controlMenu.SetActive (true);
	}
}
