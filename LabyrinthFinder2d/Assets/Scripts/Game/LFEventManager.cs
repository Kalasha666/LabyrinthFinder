using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFEventManager {

	public delegate void GetCoin();
	public static GetCoin coin;

	public delegate void GameOver();
	public static GameOver gameOver;

	public delegate void ExitGame();
	public static ExitGame exitGame;

	public delegate void DayState(DayState state);
	public static DayState dayState;


	public static void CoinDidTake()
	{
		if(coin != null)
		{
			coin ();
		}
	}

	public static void PlayDidDie()
	{
		if (gameOver != null) {
			gameOver ();
		}
	}

	public static void PlayDidExit()
	{
		if (exitGame != null) {
			exitGame ();
		}
	}

	public static void DayStateDidChange(DayState state)
	{
		if (dayState != null) {
			dayState (state);
		}
	}

	public static void RemoveAll()
	{
		coin = null;
		gameOver = null;
	}
}
