using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LFAnimalState {walk, run}

public class LFAnimalMove : MonoBehaviour {

	public float health = 1.0f;
	public float damage = 1.0f;
	public float speed = 1.0f;
	public float speedScaleFactor = 1.0f;
	public float runTime = 2.0f;
	public LFPathFinder pathFinder;
	public GameObject sprite;
	public GameObject bloodPrefab;
	public GameObject dropItemPrefab;
	public AudioClip[] sounds;
	public bool isDrawGizmos = false;
	public Color gizmosColor = Color.blue;
	private Vector3 _targetPosition;
	private List<LFGridNode> _path;
	private LFGridNode _targetNode;
	private int _pathStepIndex = 0;
	private Animator _anim;
	private LFAnimalState _state = LFAnimalState.walk;
	private float _timeRun = 3.0f;
	private float _moveSpeed = 0.0f;

	public LFAnimalState State
	{
		get{return _state;}
		set{_state = value;}
	}
		
	public void Hit(float theDamage)
	{
		Debug.Log ("Animal Hit");
		_moveSpeed = speed;
		health -= theDamage;

		_timeRun = runTime;
		GameObject blood = Instantiate(bloodPrefab, _targetPosition, Quaternion.identity);
		blood.transform.parent = gameObject.transform;

		if (health <= 0) {
			GetComponent<AudioSource> ().PlayOneShot (sounds [1]);
			StartCoroutine (Die (blood));

		} else {
			GetComponent<AudioSource> ().PlayOneShot(sounds[0]);
			StartCoroutine (RemoveBlood (blood));
		}
	}

	// Use this for initialization
	void Start () {
		_timeRun = 3.0f;
		_state = LFAnimalState.walk;
		_targetPosition = transform.position;
		_anim = gameObject.GetComponent<Animator> ();
		UpdatePath();
	}

	// Update is called once per frame
	void Update () {

		if (_targetPosition != null && !EnemyOnTargetNode()) {
			transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speedScaleFactor * speed * Time.deltaTime);
		}
		else 
		{

			switch(_state)
			{
			case LFAnimalState.run:
				_moveSpeed = speed * speedScaleFactor;

				break;
			default:
				_moveSpeed = speed;
				break;
			}

			if((_path.Count- 1) > _pathStepIndex)
			{
				_pathStepIndex +=1;
				_targetNode = _path[_pathStepIndex];
				_targetPosition = new Vector3(_targetNode.WorldPosition.x , _targetNode.WorldPosition.y, transform.position.z);
			}
			else 
			{
				UpdatePath();
			}
		}

		UpdateSprite();

		if (_state == LFAnimalState.run && _timeRun <= 0) {
			_state = LFAnimalState.walk;
			_timeRun = runTime;
		} else if (_state == LFAnimalState.run && _timeRun > 0) {
			_timeRun -= Time.deltaTime;
		}
	}

	private bool EnemyOnTargetNode()
	{
		if(_targetNode == pathFinder.GetGridNodeWithPosition(transform.position))
		{
			return true;
		}

		return false;
	}

	private void UpdatePath()
	{
		_path = pathFinder.RandomNodePath(transform.position);
		_pathStepIndex = 0;

		if(_path != null && _path.Count > 0)
		{
			_targetNode = _path[_pathStepIndex];
			_targetPosition = new Vector3(_targetNode.WorldPosition.x , _targetNode.WorldPosition.y, transform.position.z);
		}
	}

	private void UpdateSprite()
	{
		if((transform.position.x - _targetPosition.x) > 0 && sprite != null)
		{
			sprite.GetComponent<SpriteRenderer> ().flipX = true;
		}
		else if((transform.position.x - _targetPosition.x) < 0 && sprite != null)
		{
			sprite.GetComponent<SpriteRenderer> ().flipX = false;
		}	
	}

	void OnCollisionEnter(Collision collision) {
		
//		if(collision.gameObject.tag == "tWall")
//		{
//			_path = pathFinder.RandomNodePath(transform.position);
//
//			if(_path != null  && _path.Count > 0)
//				_targetNode = _path[0];
//		}
	}

	void OnDrawGizmos()
	{
		if(!isDrawGizmos) return;

		Gizmos.color = gizmosColor;
		Gizmos.DrawSphere(transform.position, 0.5f);

		if(_path != null)
		{
			foreach(LFGridNode node in _path)
			{
				Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * node.Size);
			}
		}

		if(_targetNode != null)
		{
			Gizmos.DrawCube(_targetNode.WorldPosition, Vector3.one * _targetNode.Size);
		}	

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "tPlayerAttack") {
			_state = LFAnimalState.run;
			_timeRun = runTime;
			UpdatePath();
			/*GameObject blood = Instantiate(bloodPrefab, _targetPosition, Quaternion.identity);
			blood.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			StartCoroutine (GameOver ());*/
			//other.gameObject.transform.parent.gameObject.GetComponent<LFPlayer> ().Hit (damage);
		} 
	}

	private IEnumerator GameOver ()
	{
		yield return new WaitForSeconds (1.0f);
		LFEventManager.PlayDidDie();
	}

	private IEnumerator RemoveBlood (GameObject blood)
	{
		yield return new WaitForSeconds (0.5f);
		Destroy (blood);
	}

	private IEnumerator Die (GameObject blood)
	{
		yield return new WaitForSeconds (2.5f);
		gameObject.GetComponent<BoxCollider> ().enabled = false;
		Destroy (blood);

		if (health <= 0) {
			GameObject food = Instantiate(dropItemPrefab, gameObject.transform.position, Quaternion.identity);
			food.transform.parent = gameObject.transform.parent;
			Destroy (gameObject);
		}
	}
}
