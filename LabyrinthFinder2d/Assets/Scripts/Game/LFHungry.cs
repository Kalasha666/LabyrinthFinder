using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFHungry : MonoBehaviour {

	public float timeInterval;
	public float stamina;
	private float _time;
	// Use this for initialization
	void Start () {
		_time = timeInterval;
	}
	
	// Update is called once per frame
	void Update () {
		if (_time <= 0) {
			_time = timeInterval;
			float playerStamina = gameObject.GetComponent<LFPlayer> ().stamina;

			if (playerStamina <= 0) {
				gameObject.GetComponent<LFPlayer> ().MinusHealth(1);
			} else {
				playerStamina -= stamina;
				gameObject.GetComponent<LFPlayer> ().stamina -= stamina;
			}
		}

		_time -= Time.deltaTime;
	}
}
