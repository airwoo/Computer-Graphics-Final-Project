using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class applepick : MonoBehaviour {
	public Transform respawn;
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter(Collider other){
		//Debug.Log (other.tag);
		if (other.CompareTag("Apple")) {
			StartCoroutine (eatapple (other.gameObject));
		}
		//if (other.CompareTag ("Player")) {
		//	other.gameObject.transform.position = respawn.position;
		//}
	}
	void OnCollisionEnter(Collision other){
		//Debug.Log (other.gameObject.tag);
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.transform.position = respawn.position;
		}
	}

	IEnumerator eatapple(GameObject ap){
		yield return new WaitForSecondsRealtime (15);
		ap.transform.position = new Vector3(1000,1000,1000);
	}
}
