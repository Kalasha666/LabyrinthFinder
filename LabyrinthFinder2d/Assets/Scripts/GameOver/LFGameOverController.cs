using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LFData;
using LFInput;

public class LFGameOverController : MonoBehaviour {

	public Text coinLabel;
	public Text timeLabel;
	private LFUserInput _userInput;
	private LFSessionManager _sessionManager;
	// Use this for initialization
	void Start () {
		_sessionManager = GameObject.FindGameObjectWithTag ("tSession").GetComponent<LFSessionManager>();
		_userInput = new LFUserInput ();
		_userInput.exitAction += CancelAction;

		ShowResult();
	}
	
	// Update is called once per frame
	void Update () {
		if (_userInput != null) {
			_userInput.Update ();
		}
	}

	void OnDestroy() {
		RemoveUserInput ();
	}

	private void RemoveUserInput()
	{
		if (_userInput != null) {
			_userInput.Clean ();
			_userInput = null;
		}
	}

	private void ShowResult()
	{
		if(coinLabel != null && _sessionManager != null && _sessionManager.GetCurrentSession() != null)
		{
			LFGameSession session = _sessionManager.GetCurrentSession();
			coinLabel.text = "COINS = "+  session.CoinCount;
		}

		if(timeLabel != null && _sessionManager != null && _sessionManager.GetCurrentSession() != null)
		{
			LFGameSession session = _sessionManager.GetCurrentSession();
			timeLabel.text = "Time = "+  (int)session.GameTime;
		}
	}

	// GUI Actions

	public void CancelAction()
	{
		GetComponent<AudioSource>().Play();
		SceneManager.LoadScene ("MainMenu");
	}

	public void ExitAction()
	{
		SceneManager.LoadScene ("MainMenu");
	}

}
