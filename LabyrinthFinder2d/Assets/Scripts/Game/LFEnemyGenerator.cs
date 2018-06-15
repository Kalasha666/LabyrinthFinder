using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFEnemyGenerator : MonoBehaviour {

	public float startZombieSpeed = 1.0f;
	public float startMommySpeed = 2.0f;
	public GameObject player;
	public GameObject zombiePrefab;
	public GameObject mummyPrefab;
	public GameObject enemyContainer;

	private LFLabyrinthGeneration _labyrinth;
	private LFPathFinder _pathFinder;
	private List<GameObject> _enemyList;
	// Use this for initialization
	void Start () {
		_labyrinth = gameObject.GetComponent<LFLabyrinthGeneration>();
		_pathFinder = gameObject.GetComponent<LFPathFinder>();
		_enemyList = new List<GameObject>();
	}

	public void CreateZombie()
	{
		if(_labyrinth == null || player == null || zombiePrefab == null) return;

		LFLabyrinthNode spawnNode = _labyrinth.RandomFreeNodeWithOutPosition(player.transform.position);
		GameObject zombie = Instantiate(zombiePrefab, spawnNode.WorldPosition, Quaternion.identity);
		zombie.transform.parent = enemyContainer.transform;
		zombie.GetComponent<LFEnemyMove>().pathFinder = _pathFinder;
		zombie.GetComponent<LFEnemyMove>().speed = startZombieSpeed;
		_enemyList.Add(zombie);
	}

	public void CreateMummy()
	{
		if(_labyrinth == null || player == null || mummyPrefab == null) return;

		LFLabyrinthNode spawnNode = _labyrinth.RandomFreeNodeWithOutPosition(player.transform.position);
		GameObject mummy = Instantiate(mummyPrefab, spawnNode.WorldPosition, Quaternion.identity);
		mummy.transform.parent = gameObject.transform;
		mummy.GetComponent<LFEnemyMove>().pathFinder = _pathFinder;
		mummy.GetComponent<LFEnemyMove>().speed = startMommySpeed;
		_enemyList.Add(mummy);
	}

	public void FindPlayerMode()
	{
		if(player == null) return;

		foreach(GameObject enemy in _enemyList)
		{
			enemy.GetComponent<LFEnemyMove>().Player = player.transform;
			enemy.GetComponent<LFEnemyMove>().TargetType = LFTargetType.player;
		}
	}

	public void AddSpeed(float multiplierSpeed)
	{
		foreach(GameObject enemy in _enemyList)
		{
			enemy.GetComponent<LFEnemyMove>().speed *= multiplierSpeed;
		}
	}
}
