using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LFData;
using LFInput;

public class LFMainMenuController : MonoBehaviour {

	private LFSessionManager _sessionManager;
	private LFUserInput _userInput;
	// Use this for initialization
	void Start () {
		_userInput = new LFUserInput ();
		_userInput.exitAction += Exit;
		_sessionManager = GameObject.FindGameObjectWithTag ("tSession").GetComponent<LFSessionManager>();

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

	private void Exit()
	{
		Application.Quit ();
	}

	// GUI Actions

	public void StartAction()
	{
		GetComponent<AudioSource>().Play();
		SceneManager.LoadScene ("GameWeekend");
	}

	public void PlayerAction()
	{
		GetComponent<AudioSource>().Play();
		SceneManager.LoadScene ("Player");
	}

	public void LeadboardAction()
	{
		GetComponent<AudioSource>().Play();
		SceneManager.LoadScene ("Leadboard");
	}

	public void ExitAction()
	{
		Exit ();
	}
}
