using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LFInput;

public class LFMove : MonoBehaviour {

	public enum LFPlayerState { indle, left, right, top, down, dead}
	public float speed = 2.0f;
	private LFPlayerState _state = LFPlayerState.indle;
	public GameObject sprite;
	private Animator _anim;
	private LFUserInput _userInput;
	private Vector3 _direction;
	// Use this for initialization
	void Start () {
		_anim = gameObject.GetComponent<Animator> ();
		_userInput = new LFUserInput ();
	}
	
	// Update is called once per frame
	void Update () {
		_userInput.Update ();
		_direction = _userInput.Direction;
		Move ();
		UpdatePlayerState();
	}

	private void Move()
	{
		//Debug.Log("Direction " + _direction);
		transform.Translate (_direction * speed * Time.deltaTime);
	}

	private void UpdatePlayerState()
	{
		if(_direction.x < 0)
		{
			_state =  LFPlayerState.left;
		}
		else if(_direction.x > 0)
		{
			_state =  LFPlayerState.right;
		}
		else if (_direction.x == 0 && _direction.y > 0 )
		{
			_state =  LFPlayerState.top;
		}
		else if (_direction.x == 0 && _direction.y < 0 )
		{
			_state =  LFPlayerState.down;
		}
		else if (_direction.Equals(Vector3.zero))
		{
			_state =  LFPlayerState.indle;
		}


		switch (_state) {
		case LFPlayerState.left:
			sprite.GetComponent<SpriteRenderer> ().flipX = true;
			_anim.Play ("SurvikiMove");
			break;
		case LFPlayerState.right:
			sprite.GetComponent<SpriteRenderer> ().flipX = false;
			_anim.Play ("SurvikiMove");
			break;
		case LFPlayerState.top:
			_anim.Play ("SurvikiMove");
			break;
		case LFPlayerState.down:
			_anim.Play ("SurvikiMove");
			break;
		case LFPlayerState.indle:
			//_anim.Stop ();
			break;
		default:
			//_anim.Stop ();
			break;
		}
	}
}
