using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFAnimalGenerator : MonoBehaviour {

	public float startPingSpeed = 1.0f;
	public GameObject pingPrefab;
	public GameObject animalContainer;

	private LFLabyrinthGeneration _labyrinth;
	private LFPathFinder _pathFinder;
	private List<GameObject> _animalList;
	// Use this for initialization
	void Start () {
		_labyrinth = gameObject.GetComponent<LFLabyrinthGeneration>();
		_pathFinder = gameObject.GetComponent<LFPathFinder>();
		_animalList = new List<GameObject>();
	}

	void Update()
	{
		
	}

	public void CreatePing()
	{
		if(_labyrinth == null || pingPrefab == null) return;

		LFLabyrinthNode spawnNode = _labyrinth.RandomFreeNode();
		GameObject pig = Instantiate(pingPrefab, spawnNode.WorldPosition, Quaternion.identity);
		pig.transform.parent = animalContainer.transform;
		pig.GetComponent<LFAnimalMove>().pathFinder = _pathFinder;
		pig.GetComponent<LFAnimalMove>().speed = startPingSpeed;
		pig.SetActive (true);
		_animalList.Add(pig);
	}
}
