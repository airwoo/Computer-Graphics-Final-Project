using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Director : MonoBehaviour {
	public Camera camera;
	public float speed;
	public Rigidbody obstacle;
	public Transform maincamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") ){
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag.CompareTo ("obstacle") == 0) {
					obstacle = hit.transform.GetComponent<Rigidbody> ();
				} else if (hit.transform.CompareTag ("Player")) {
					hit.transform.GetComponent<NavMeshAgent> ().enabled = !hit.transform.GetComponent<NavMeshAgent> ().enabled;
				} else {
					foreach (Transform child in transform) {
						child.GetComponent<NavMeshAgent> ().SetDestination (hit.point);
						child.GetComponent<AgentBehaviour> ().destination = hit.point;
					}
				}
			}
		}
		if (Input.GetButtonDown("Fire2") ){
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.CompareTag ("Player")) {
					maincamera.GetComponent<IOCamera> ().player = hit.transform;
				} 
			}
		}

	}

	void FixedUpdate(){
		float moveHor = 0.0f;
		float moveVer = 0.0f;
		moveHor = Input.GetAxis ("Horizontal2");
		moveVer = Input.GetAxis ("Vertical2");
		Vector3 movement = new Vector3 (moveHor, 0.0f, moveVer);
		obstacle.AddForce (movement * speed);
	}
}
