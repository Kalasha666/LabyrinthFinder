using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFTree : MonoBehaviour {

	public float health = 5.0f;
	public GameObject effectPrefab;
	public GameObject dropItemPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hit(float theDamage)
	{
		Debug.Log ("Tree Hit");
		health -= theDamage;
		GameObject effect = Instantiate(effectPrefab, gameObject.transform.position, Quaternion.identity);
		effect.transform.parent = gameObject.transform;
		StartCoroutine (RemoveEffect (effect));
	}

	private IEnumerator RemoveEffect (GameObject effect)
	{
		yield return new WaitForSeconds (0.5f);
		Destroy (effect);

		if (health <= 0) {
			GameObject food = Instantiate(dropItemPrefab, gameObject.transform.position, Quaternion.identity);
			food.transform.parent = gameObject.transform.parent;
			Destroy (gameObject);
		}
	}
}
