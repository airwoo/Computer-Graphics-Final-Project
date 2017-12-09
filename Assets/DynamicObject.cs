using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DynamicObject : MonoBehaviour {
	public Vector3 pointA;
	public Vector3 pointB;
	private Vector3 target;
	private bool turn;
	private NavMeshAgent ag;
	// Use this for initialization
	void Start () {
		turn = false;
		target = pointB;
		ag = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		ag.SetDestination (target);
		float distance = Vector3.Distance (this.transform.position, target);
		if (distance < 0.2f) {
			turn = !turn;
			if (turn) {
				target = pointA;
			} else {
				target = pointB;
			}
		}
	}
}
