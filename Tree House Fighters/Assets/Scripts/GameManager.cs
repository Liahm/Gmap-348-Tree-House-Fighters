using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "GameManager";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

	public float PlayerOneHealth, PlayerTwoHealth;
	public PlayerMovement PlayerOne, PlayerTwo;
	public Slider PlayerOneHealthBar, PlayerTwoHealthBar;
	public Text PlayerOneWinner, PlayerTwoWinner;
	public Text PlayerOneDefeat, PlayerTwoDefeat;
	public Button MainMenu, Restart;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		PlayerOneHealthBar.maxValue = PlayerOneHealth;
		PlayerTwoHealthBar.maxValue = PlayerTwoHealth;
		PlayerOneHealthBar.value = PlayerOneHealth;
		PlayerTwoHealthBar.value = PlayerTwoHealth;
	}
		
	void Update()
    {
		if(PlayerOneHealthBar.value <= 0)
		{
			PlayerTwoWinner.gameObject.SetActive(true);
			PlayerOneDefeat.gameObject.SetActive(true);
			EndOfGame();
		}
		else if(PlayerTwoHealthBar.value <= 0)
		{
			PlayerOneWinner.gameObject.SetActive(true);
			PlayerTwoDefeat.gameObject.SetActive(true);
			EndOfGame();
		}
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	private void EndOfGame()
	{
		PlayerOne.CanMove = false;
		PlayerTwo.CanMove = false;
		PlayerOne.CanShoot = false;
		PlayerTwo.CanShoot = false;

		Restart.gameObject.SetActive(true);
		MainMenu.gameObject.SetActive(true);
	}
}