using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFForest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "tPlayer") {
			gameObject.GetComponent<AudioSource> ().enabled = true;
		} 
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "tPlayer") {
			gameObject.GetComponent<AudioSource> ().enabled = false;
		} 
	}
}
