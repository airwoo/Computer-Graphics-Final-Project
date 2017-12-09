using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class directorTest: MonoBehaviour {
	public Camera camera;
	public float speed;
	public Rigidbody obstacle;
	public NavMeshAgent	agent;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") ){
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
					agent.SetDestination (hit.point);
					
				}
			}
		}


	}



