using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	
	
	void Strt () {
		SceneManager.LoadScene("SampleScene");
	}
	
	void BackToMain () {
		SceneManager.LoadScene("EVO Main Menu");
	}
	
	void Quit () {
		Application.Quit();
	}
}