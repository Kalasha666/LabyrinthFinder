using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LFLeadboardRow : MonoBehaviour {
	public Text idSession;
	public Text playerName;
	public Text coinCount;
	public Text gameTime;
	public Text startTime;
	public Text exitType;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetValue(string newIdSession, string newPlayerName,
		string newCoinCount, string newGameTime, string newStartTime, string newExitType)
	{
		idSession.text = newIdSession;
		playerName.text = newPlayerName;
		coinCount.text = newCoinCount;
		gameTime.text = newGameTime;
		startTime.text = newStartTime;
		exitType.text = newExitType;
	}
}
