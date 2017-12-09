using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AgentBehaviour : MonoBehaviour {
	public Vector3 destination;
	NavMeshAgent agent;
	private RaycastHit hit;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();

	}
	
	// Update is called once per frame
	/*void Update () {
		if (Input.GetButtonDown("Fire1") ){
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				agent.SetDestination (hit.point);
			}
		}


	}*/
	void FixedUpdate()
	{
		//Collider[] hitColliders = Physics.OverlapSphere (transform.position, 10.0f);
		float distance = Vector3.Distance(this.transform.position, destination);
		if (transform.name.CompareTo ("Agent") == 0) {
			Debug.Log (agent.velocity);
		}
		if (distance > 5.0f) {
			agent.isStopped = false;
			return;
		}
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.0f);
		int i = 0;
		while (i < hitColliders.Length) {
			float angle = Vector3.Dot (fwd, (hitColliders [i].transform.position-this.transform.position).normalized);
			float degree = Mathf.Acos (angle) * Mathf.Rad2Deg;
			if (degree < -45.0f || degree > 45.0f) {
				i++;
				continue;
			}
			if (hitColliders [i].gameObject.CompareTag ("Player")) {
				agent.isStopped = true;
				return;
			}
			i++;
		}
		agent.isStopped = false;
	}
}
