using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LFTimeInfo : MonoBehaviour {

	public Text globalTime;
	public Text dayTime;
	public Text dayState;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetInfo(int theGlobalTime, int theDayTime, string theState)
	{
		globalTime.text = "GlobalTime =" + theGlobalTime;
		dayTime.text = "DayTime =" + theDayTime;
		dayState.text = "DaySate =" + theState;
	}
}
