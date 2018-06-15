using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFCoinGenerator : MonoBehaviour {

	public float genTime = 5.0f;
	public GameObject coinPrefab;
	public int maxCount = 10;

	private List<GameObject> _coins;
	private List<GameObject> _deleteCoins;
	private float _time = 0.0f;
	private LFLabyrinthGeneration _labyrinth;
	// Use this for initialization
	void Start () {
		_coins = new List<GameObject> ();
		_deleteCoins = new List<GameObject> ();
		_labyrinth = gameObject.GetComponent<LFLabyrinthGeneration>();
	}
	
	// Update is called once per frame
	void Update () {
		_time -= Time.deltaTime;

		if (_time <= 0 && _coins.Count < maxCount && coinPrefab != null) {
			_time = genTime;
			CreateCoin ();
		}

		RemoveTakenCoins ();
	}

	private void CreateCoin()
	{
		if(_labyrinth == null) return;

		GameObject coin = Instantiate (coinPrefab, gameObject.transform);
		LFLabyrinthNode spawnNode = _labyrinth.RandomFreeNode();
		//int z = 0;
		coin.transform.position =spawnNode.WorldPosition; //new Vector3 (spawnNode.WorldPosition.x, spawnNode.WorldPosition.y, z);
		_coins.Add (coin);
	}

	private void RemoveTakenCoins()
	{
		foreach (GameObject coin in _coins) {
			if (!coin.activeSelf) {
				_deleteCoins.Add(coin);
			}
		}

		foreach (GameObject coin in _deleteCoins) {
			_coins.Remove(coin);
			Destroy(coin);
		}

		_deleteCoins.Clear();
	}
}
