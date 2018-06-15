using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LFInput;

public class LFPlayer : MonoBehaviour {

	public float health = 3.0f;
	public float stamina = 10.0f;
	public float damage = 1.0f;
	public int food = 5;
	public int wood = 5;
	public GameObject blood;
	public GameObject attackZone;
	public AudioClip[] sounds;
	private Animator _anim;
	private List<GameObject> _collidedEnemy;
	private LFUserInput _userInput;
	// Use this for initialization
	void Start () {
		_anim = gameObject.GetComponent<Animator> ();
		_collidedEnemy = new List<GameObject> ();
		_userInput = new LFUserInput ();
		_userInput.attackAction += Attack;
		_userInput.attackEndAction += EndAttack;
		_userInput.eatAction += Eat;
	}
	
	// Update is called once per frame
	void Update () {
		_userInput.Update ();
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter " + other.gameObject.tag + " collidedEnemy =" + _collidedEnemy.Count);
		if (other.gameObject.tag == "tEnemy" || other.gameObject.tag == "tAnimal" || other.gameObject.tag == "tTree") {
			
			_collidedEnemy.Add (other.gameObject);
			Debug.Log ("!!!OnTriggerEnter " + other.gameObject.tag + " collidedEnemy =" + _collidedEnemy.Count);
		} else if (other.gameObject.tag == "tFood" && other.gameObject.activeSelf) {
			other.gameObject.SetActive (false);
			Destroy (other.gameObject);
			food += 1;
		}
		else if (other.gameObject.tag == "tWood" && other.gameObject.activeSelf) {
			other.gameObject.SetActive (false);
			Destroy (other.gameObject);
			wood += 1;
		}

		/*LFEventManager.CoinDidTake ();
			gameObject.GetComponent<AudioSource>().Play();
			other.gameObject.SetActive (false);*/
 	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("!!!OnTriggerExit " + other.gameObject.tag + " collidedEnemy =" + _collidedEnemy.Count);

		if (other.gameObject.tag == "tEnemy" || other.gameObject.tag == "tAnimal" || other.gameObject.tag == "tTree") {
			_collidedEnemy.Remove (other.gameObject);

			Debug.Log ("!!!OnTriggerExit " + other.gameObject.tag + " collidedEnemy =" + _collidedEnemy.Count);
		} 

		/*LFEventManager.CoinDidTake ();
			gameObject.GetComponent<AudioSource>().Play();
			other.gameObject.SetActive (false);*/
	}

	public void Hit(float theDamage)
	{
		health -= theDamage;
		GameObject bloodEffect = Instantiate(blood, gameObject.transform.position, Quaternion.identity);
		//blood.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		bloodEffect.transform.parent = gameObject.transform;
		StartCoroutine (EndHit (bloodEffect));
	}

	public void MinusHealth(float dHealth)
	{
		health -= dHealth;

		if (health <= 0) {
			LFEventManager.PlayDidDie();
			gameObject.SetActive (false);
		}
	}

	private IEnumerator EndHit (GameObject blood)
	{
		yield return new WaitForSeconds (2.0f);

		if (blood != null) {
			Destroy (blood);
		}

		if (health <= 0) {
			LFEventManager.PlayDidDie();
			gameObject.SetActive (false);
		}
	}

	private void Attack()
	{
		Debug.Log ("Attack");

		attackZone.SetActive (true);
		_anim.Play ("SurvikiAttack");

		for(int i = _collidedEnemy.Count - 1; i >= 0; i-- )
		{
			GameObject enemy = _collidedEnemy[i];

			if(enemy == null)
			{
				_collidedEnemy.Remove(enemy);
			}
		}

		foreach (GameObject enemy in _collidedEnemy) {
			if (enemy.tag == "tEnemy") {
				enemy.transform.parent.GetComponent<LFEnemyMove>().Hit(damage);
			} else if (enemy.tag == "tAnimal") {
				enemy.transform.parent.GetComponent<LFAnimalMove>().Hit(damage);
			} else if (enemy.tag == "tTree") {
				enemy.transform.GetComponent<LFTree>().Hit(damage);
			}
		}

		if (_collidedEnemy.Count > 0) {
			GetComponent<AudioSource> ().PlayOneShot (sounds [1]);
		} else {
			GetComponent<AudioSource> ().PlayOneShot (sounds [0]);
		}
	}

	private void EndAttack()
	{
		attackZone.SetActive (false);

		for(int i = _collidedEnemy.Count - 1; i >= 0; i-- )
		{
			GameObject enemy = _collidedEnemy[i];

			if(enemy == null)
			{
				_collidedEnemy.Remove(enemy);
			}
		}
	}

	private void Eat()
	{
		if (food > 0 && health < 3) {
			food -= 1;
			health += 1;
			GetComponent<AudioSource> ().PlayOneShot(sounds[2]);
		}
	}
}
