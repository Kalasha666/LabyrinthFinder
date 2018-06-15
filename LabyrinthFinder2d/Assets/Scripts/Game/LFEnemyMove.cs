using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LFTargetType {random, player}

public class LFEnemyMove : MonoBehaviour {

	public float health = 1.0f;
	public float damage = 1.0f;
	public float speed = 2.0f;
	public LFPathFinder pathFinder;
	public GameObject sprite;
	public GameObject bloodPrefab;
	public AudioClip[] sounds;
	public bool isDrawGizmos = false;
	public Color gizmosColor = Color.blue;
	private Transform _player;
	private Vector3 _targetPosition;
	private List<LFGridNode> _path;
	private LFGridNode _targetNode;
	private int _pathStepIndex = 0;
	private Animator _anim;
	private LFTargetType _targetType = LFTargetType.random;

	public LFTargetType TargetType
	{
		get{return _targetType;}
		set{_targetType = value;}
	}

	public Transform Player
	{
		get{return _player;}
		set{_player = value;}
	}

	public void Hit(float theDamage)
	{
		Debug.Log ("Enemy Hit");
		GetComponent<AudioSource> ().PlayOneShot(sounds[1]);
		health -= theDamage;
		GameObject blood = Instantiate(bloodPrefab, _targetPosition, Quaternion.identity);
		blood.transform.parent = gameObject.transform;
		StartCoroutine (RemoveBlood (blood));
	}

	// Use this for initialization
	void Start () {
		_targetType = LFTargetType.random;
		_targetPosition = transform.position;
		_anim = gameObject.GetComponent<Animator> ();
		UpdatePath();
	}

	// Update is called once per frame
	void Update () {

		if (_targetPosition != null && !EnemyOnTargetNode()) {
			transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
		}
		else 
		{
			switch(_targetType)
			{
			case LFTargetType.player:
				UpdatePath();

				break;
			default:

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

				break;
			}
		}

		UpdateSprite();
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
		switch(_targetType)
		{
		case LFTargetType.player:
			_path = pathFinder.FindPath (transform.position,  _player.position);
			_pathStepIndex = 0;

			break;
		default:
			
			_path = pathFinder.RandomNodePath(transform.position);
			_pathStepIndex = 0;

			break;
		}

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
			sprite.GetComponent<SpriteRenderer> ().flipX = false;
		}
		else if((transform.position.x - _targetPosition.x) < 0 && sprite != null)
		{
			sprite.GetComponent<SpriteRenderer> ().flipX = true;
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
		if (other.gameObject.tag == "tPlayer") {
			_targetType = LFTargetType.player;
			_player = other.gameObject.transform;
			_targetPosition = other.gameObject.transform.position;
			UpdateSprite();
			_anim.Play("Attack");
			gameObject.GetComponent<AudioSource>().Play();
			/*GameObject blood = Instantiate(bloodPrefab, _targetPosition, Quaternion.identity);
			blood.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			StartCoroutine (GameOver ());*/
			GetComponent<AudioSource> ().PlayOneShot(sounds[0]);
			other.gameObject.transform.parent.gameObject.GetComponent<LFPlayer> ().Hit (damage);
		} 
	}

	private IEnumerator GameOver ()
	{
		yield return new WaitForSeconds (1.0f);
		LFEventManager.PlayDidDie();
	}

	private IEnumerator RemoveBlood (GameObject blood)
	{
		yield return new WaitForSeconds (1.0f);
		Destroy (blood);

		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}
