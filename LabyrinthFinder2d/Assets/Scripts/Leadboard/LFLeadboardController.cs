using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LFData;
using LFInput;

public class LFLeadboardController : MonoBehaviour {

	public Transform content;
	public GameObject rowPrefab;

	private LFUserInput _userInput;
	private List<GameObject> _rowList;
	private LFSessionManager _sessionManager;
	// Use this for initialization
	void Start () {
		_rowList = new List<GameObject> ();
		_sessionManager = GameObject.FindGameObjectWithTag ("tSession").GetComponent<LFSessionManager>();

		if (_sessionManager != null && _sessionManager.GetCurrentSession() != null) {
			CreateLeadboardRows ();
		}

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

	private void CreateLeadboardRows()
	{
		foreach (GameObject row in _rowList) {
			Destroy (row);
		}

		List<LFGameSession> sessions = _sessionManager.GetSessions ();

		for (int i = 0; i < sessions.Count; i++) {
			LFGameSession session = sessions [i];
			GameObject row = Instantiate (rowPrefab,  content) as GameObject;
			LFLeadboardRow rowScript = row.GetComponent<LFLeadboardRow> ();
			rowScript.SetValue (i.ToString(), session.PlayerName, session.CoinCount.ToString(), session.GameTime.ToString(),
				session.StartDate, session.ExitType.ToString());
			_rowList.Add (row);
		}

	/*	for (int i = 0; i < 2; i++) {
			GameObject row = Instantiate (rowPrefab,  content) as GameObject;
			LFLeadboardRow rowScript = row.GetComponent<LFLeadboardRow> ();
			row.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); 
			_rowList.Add (row);
		}*/
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

	public void CleanAction()
	{
		GetComponent<AudioSource>().Play();

		if (_sessionManager != null && _sessionManager.GetCurrentSession() != null) {
			_sessionManager.Clean ();
			_sessionManager.Save ();
			CreateLeadboardRows ();
		}
	}
}
