using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public bool open;
	private Vector3 goal, start;
	private Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
		open = false;
		start = transform.position;
		goal = transform.position + transform.up * 15;
	}
	
	// Update is called once per frame
	void Update () {
		if (open) {
			transform.position = Vector3.SmoothDamp (transform.position, goal, ref velocity, 1.9f);
		} 
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			open = true;
		}
	}

}
