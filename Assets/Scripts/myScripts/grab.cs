using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Animator))]

public class grab : MonoBehaviour {

	protected Animator animator;
	public bool ikActive = false;
	public Transform rightHandObj = null;
	public Transform lookObj = null;

	private GameObject triggeringApple;
	private bool inRange;
	public GameObject grabText;
	public Text grabChangeText;
	public GameObject Erika;

	private readonly int iG = Animator.StringToHash("isGrab");
	private readonly int iThrow = Animator.StringToHash("isThrow");

	public Rigidbody rgb; 
	void Start(){
		animator = GetComponent<Animator> ();

	}



	void Update(){
		if (inRange) {
			grabText.SetActive (true);
			if (Input.GetKeyDown ("g") == true) {
				if (triggeringApple.tag == "Apple") {
					animator.SetBool (iG, true);
					rgb.constraints = RigidbodyConstraints.FreezeAll;
					triggeringApple.GetComponent<SphereCollider> ().enabled = false;
					StartCoroutine (GrabApple ());

				}

			} 

		}
		else {
			grabText.SetActive (false);
		}
		if (Input.GetKeyDown ("h") == true) {
				animator.SetBool (iThrow, true);
				rgb.constraints = RigidbodyConstraints.None;
		} 

	    if (Input.GetKeyUp ("h") == true) {
			animator.SetBool (iThrow, false);
		}
	}
	void OnAnimatorIK(){
		if(animator) {

			//if the IK is active, set the position and rotation directly to the goal. 
			if(ikActive) {

				// Set the look target position, if one has been assigned
				if(lookObj != null) {
					animator.SetLookAtWeight(1);
					animator.SetLookAtPosition(lookObj.position);
				}    

				// Set the right hand target position and rotation, if one has been assigned
				if(rightHandObj != null) {
					animator.SetIKPositionWeight (AvatarIKGoal.RightHand, 1);
					animator.SetIKRotationWeight (AvatarIKGoal.RightHand, 1);
					animator.SetIKPosition (AvatarIKGoal.RightHand, rightHandObj.position);
					animator.SetIKRotation (AvatarIKGoal.RightHand, rightHandObj.rotation);
				}        

			}

			//if the IK is not active, set the position and rotation of the hand and head back to the original position
			else {          
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
				animator.SetLookAtWeight(0);
			}
		}


	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Apple") {
			inRange = true;
			triggeringApple = other.gameObject;
		}
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "Apple") {
			inRange = false;
			triggeringApple = null;
			grabChangeText.text = "";
		}
	}

	IEnumerator GrabApple(){
		yield return new WaitForSecondsRealtime (2);
		triggeringApple.transform.parent = rightHandObj.transform;
		triggeringApple.transform.position = rightHandObj.transform.position;

	}
}
