using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LFPlayerInfo : MonoBehaviour {

	public Text health;
	public Text stamina;
	public Text damage;
	public Text speed;
	public Text food;
	public Text wood;
	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		if (health != null && stamina != null && damage != null && speed != null && player != null) {
			health.text = "Health =" + player.GetComponent<LFPlayer> ().health;
			stamina.text = "Stamina =" + player.GetComponent<LFPlayer> ().stamina;
			damage.text = "Damage =" + player.GetComponent<LFPlayer> ().damage;
			speed.text = "Speed =" + player.GetComponent<LFMove> ().speed;
			food.text = "Food =" + player.GetComponent<LFPlayer> ().food;
			wood.text = "Wood =" + player.GetComponent<LFPlayer> ().wood;
		}

	}
}
