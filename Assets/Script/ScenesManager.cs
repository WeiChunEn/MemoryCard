using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour {

	public AudioSource btnClick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LaunchStart () {
		SceneManager.LoadScene ("mainMenu");
	}

	public void LaunchGame () {
		btnClick.Play ();
		SceneManager.LoadScene ("Game");
	}

	public void BtnClickedAudio () {
		btnClick.Play ();
	}

	public void QuitGame () {
		Application.Quit ();
	}
}
