using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	public GameObject ParentBone;
	public Rigidbody RBody;
	public float Force;
	public bool hasBeenThrown;

	void Start(){
		RBody.useGravity = false;
		hasBeenThrown = false;
	}

	void Update(){
		
	}

	public void ReleaseBall(){
		transform.parent = null;
		RBody.useGravity = true;
		transform.rotation = ParentBone.transform.rotation;
		RBody.AddForce (transform.forward * Force);
		hasBeenThrown = true;
	}
}
