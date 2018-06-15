using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LFData;
using LFInput;

public class LFGameController : MonoBehaviour {

	public Text coinCount;
	public Text time;
	private LFUserInput _userInput; 
	// Use this for initialization
	void Start () {
		_userInput = new LFUserInput ();
		_userInput.exitAction += CancelAction;
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

	public void CancelAction()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void ExitAction()
	{
		LFEventManager.PlayDidExit();
		CancelAction ();
	}

	public void SetCoinCount(int count)
	{
		coinCount.text = "COIN = "+ count;
	}

	public void SetTime(int timeCount)
	{
		time.text = "Time =" + timeCount;
	}
}
