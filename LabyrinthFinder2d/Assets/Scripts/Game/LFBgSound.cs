using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFBgSound : MonoBehaviour {

	public AudioClip[] sounds;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Day()
	{
		GetComponent<AudioSource> ().PlayOneShot(sounds[0]);
	}

	public void Night()
	{
		GetComponent<AudioSource> ().PlayOneShot(sounds[1]);
	}
}
