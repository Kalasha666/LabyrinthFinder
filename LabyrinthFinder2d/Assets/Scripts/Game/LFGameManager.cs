using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DayState {day, night}

public class LFGameManager : MonoBehaviour {

	public GameObject camera;
	public GameObject playerPrefab;
	public GameObject labyrinth;
	public LFGameController guiController;
	public LFPlayerInfo playeInfo;
	public LFTimeInfo timeInfo;
	public GameObject nightView;
	public DayState dayState = DayState.day;
	public float changeDateStateTime = 180.0f;
	public GameObject playerSpawn;
	public AudioClip[] sounds;
	private int _coins;
	private LFSessionManager _sessionManager;
	private LFEnemyGenerator _enemyGenerator;
	private LFCoinGenerator _coinGenerator;
	private LFAnimalGenerator _animalGenerator;
	private LFGrid _grid;
	private LFLabyrinthGeneration _labyrinth;
	private float _time;
	private float _dayTime;
	private GameObject _player;

	// Use this for initialization
	void Start () {
		_sessionManager = GameObject.FindGameObjectWithTag("tSession").GetComponent<LFSessionManager>();

		if(labyrinth != null)
		{
			_labyrinth = labyrinth.GetComponent<LFLabyrinthGeneration>();
			_labyrinth.StartInitLabyrinth();
			_grid = labyrinth.GetComponent<LFGrid>();
			_grid.StartInitGrid();
			_enemyGenerator = labyrinth.GetComponent<LFEnemyGenerator>();
			_coinGenerator = labyrinth.GetComponent<LFCoinGenerator>();
			_animalGenerator = labyrinth.GetComponent<LFAnimalGenerator>();
			CreatePlayer();
		}

		_time = 0.0f;
		_dayTime = changeDateStateTime;

		LFEventManager.coin += PlayerDidGetCoin;
		LFEventManager.gameOver += PlayerDidDie;

		if(_enemyGenerator != null)
		{
			//_enemyGenerator.CreateZombie();

			for (int i = 0; i < 10; i++) {
				_enemyGenerator.CreateZombie();
			}
		}

		if(_animalGenerator != null)
		{
			//_enemyGenerator.CreateZombie();

			for (int i = 0; i < 10; i++) {
				_animalGenerator.CreatePing();
			}
		}

		if(_player != null && camera != null)
		{
			camera.GetComponent<LFCameraFollow>().target = _player.transform;
			camera.GetComponent<LFCameraFollow> ().leftBottomCorner = GetBottomLeftCameraPos ();
			camera.GetComponent<LFCameraFollow> ().topRightCorner = GetTopRightCameraPos ();

		}

		if (_player != null && playeInfo != null) {
			playeInfo.player = _player;
		}

		UpdateDayState ();
	}

	void OnDestroy() {
		LFEventManager.RemoveAll();
	} 
	
	// Update is called once per frame
	void Update () {

		if (_dayTime <= 0) {
			SwitchDayState ();
			_dayTime = changeDateStateTime;
		}

		_dayTime -= Time.deltaTime;
		_time += Time.deltaTime;

		if(guiController)
			guiController.GetComponent<LFGameController> ().SetTime ((int)_time);

		if (timeInfo != null) {
			timeInfo.SetInfo ((int)_time, (int)_dayTime, dayState.ToString ());
		}
			
	}

	private void SwitchDayState()
	{
		if (dayState == DayState.day) {
			dayState = DayState.night;
			GetComponent<AudioSource> ().PlayOneShot(sounds[1]);

			if(camera != null)
			{
				camera.GetComponent<LFBgSound>().Night();
			}
		} else {
			dayState = DayState.day;
			GetComponent<AudioSource> ().PlayOneShot(sounds[0]);

			if(camera != null)
			{
				camera.GetComponent<LFBgSound>().Day();
			}
		}

		UpdateDayState ();
	}

	private void UpdateDayState()
	{
		switch (dayState) {
		case DayState.day:
			nightView.SetActive (false);
			break;

		case DayState.night:
			nightView.SetActive (true);
			break;
		}
	}

	private void CreatePlayer()
	{
		LFLabyrinthNode spawnNode = _labyrinth.GridNodeFromWorldPosition(playerSpawn.transform.position);
		_player = Instantiate(playerPrefab, spawnNode.WorldPosition, Quaternion.identity);
		_player.transform.parent = labyrinth.transform;
	}

	public Vector3 GetBottomLeftCameraPos()
	{
		Vector3 leftBottom = new Vector3 (0, 0, -10);

		if (_labyrinth != null) {
			leftBottom.x = labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.x * -0.5f + labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.x / 4 * 0.5f ;
			leftBottom.y = labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.y * -0.5f + labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.y / 3 * 0.5f;
		}

		return leftBottom;
	}

	public Vector3 GetTopRightCameraPos()
	{
		Vector3 rightTop = new Vector3 (0, 0, -10);

		if (_labyrinth != null) {
			rightTop.x = labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.x * 0.5f - labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.x / 4 * 0.5f;
			rightTop.y = labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.y * 0.5f - labyrinth.GetComponent<LFLabyrinthGeneration> ().gridWorldSize.y / 3 * 0.5f;
		}

		return rightTop;
	}

	private void PlayerDidGetCoin()
	{
		Debug.Log("PlayerDidGetCoin " + Time.deltaTime);

		if(_sessionManager != null)
		{
			_sessionManager.GetCurrentSession().CoinCount += 1;

			if(guiController != null)
			{
				guiController.SetCoinCount(_sessionManager.GetCurrentSession().CoinCount);
			}

			if(_sessionManager.GetCurrentSession().CoinCount == 5 && _enemyGenerator != null)
			{
				_enemyGenerator.CreateZombie();
			}
			else if(_sessionManager.GetCurrentSession().CoinCount == 10 && _enemyGenerator != null)
			{
				_enemyGenerator.CreateMummy();
			}
			else if (_sessionManager.GetCurrentSession().CoinCount == 20 && _enemyGenerator != null)
			{
				_enemyGenerator.FindPlayerMode();
			}
			else if (_sessionManager.GetCurrentSession().CoinCount > 20 && _enemyGenerator != null)
			{
				_enemyGenerator.AddSpeed(1.05f);
			}
		}
	}

	private void PlayerDidDie()
	{
		Debug.Log("PlayerDidDie");

		SceneManager.LoadScene("GameOver");
	}
}
