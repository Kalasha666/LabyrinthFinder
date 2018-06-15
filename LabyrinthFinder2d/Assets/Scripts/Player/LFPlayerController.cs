using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LFData;
using LFInput;

public class LFPlayerController : MonoBehaviour {

	public InputField playerName;

	private LFSessionManager _sessionManager;
	private LFUserInput _userInput;
	private LFGameSession _session;
	// Use this for initialization
	void Start () {
		_userInput = new LFUserInput ();
		_userInput.exitAction += CancelAction;
		_sessionManager = GameObject.FindGameObjectWithTag ("tSession").GetComponent<LFSessionManager>();

		if (_sessionManager != null && _sessionManager.GetCurrentSession() != null) {
			_session = _sessionManager.GetCurrentSession ();
		}
			
		playerName.text = _session.PlayerName;
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

	// GUI Actions

	public void SaveAction()
	{
		if(_session != null)
		{
			_session.PlayerName = playerName.text;
		}

		GetComponent<AudioSource>().Play();
		SceneManager.LoadScene ("MainMenu");
	}

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
