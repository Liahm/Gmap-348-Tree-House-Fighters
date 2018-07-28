using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "MainMenu";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{

	}
		
	void Update()
    {
		if(Input.GetKeyUp(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
    }

//--------------------------------------------------------------------------METHODS:
	public void StartGame()
	{
		if(SceneManager.GetActiveScene().name == "GameOver" 
			|| SceneManager.GetActiveScene().name == "Victory")
		{	
			SceneManager.LoadScene("MainMenu");	
		}
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);		
	}

	public void GoBackToMainMenu()
	{
		SceneManager.LoadScene("Main Menu");	
	}
	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Quit()
	{
		Application.Quit();
	}
//--------------------------------------------------------------------------HELPERS:
	
}