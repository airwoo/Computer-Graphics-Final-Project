using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class grab : MonoBehaviour {

	#region Data
	[SerializeField]
	Transform myRightHandMiddleFinger;
	Transform myRightHand;
	[SerializeField]
	GameObject target;

	Animator animator;
	Transform Zoe;

	public bool ikActive = false;
	public Transform rightHandObj = null;
	public Transform lookObj = null;

	private readonly int iG = Animator.StringToHash("isGrab");
	static readonly Vector3 offset = new Vector3(0.2f, .04f, 0);

	Vector3 myRightMiddleFingerPosition, myRightHandPosition;
	Quaternion myRightMiddleFingerRotation;
	#endregion

	float percentComplete{
		get{
			AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo (0);
			float percent = currentAnimation.normalizedTime % 1;
			percent *= 2;
			if (percent > 1) {
				percent = 2 - percent;
			}
			return percent;
		}
	}

	private void Awake(){
		animator = GetComponent<Animator> ();
		myRightHand = myRightHandMiddleFinger.parent;
	}



	void Update(){
		
		if (Input.GetKeyDown ("g") == true) {
			
			animator.SetBool (iG, true);
			myRightMiddleFingerPosition = myRightHandMiddleFinger.position;
			myRightMiddleFingerRotation = myRightHandMiddleFinger.rotation;
			myRightHandPosition = myRightHand.position;

		} 
		if (Input.GetKeyDown ("1") == true) {
			ikActive = true;

		} 
		if (Input.GetKeyDown ("2") == true) {
			ikActive = false;

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
					Vector3 targetPosition = target.transform.position;
					targetPosition += myRightMiddleFingerPosition + myRightMiddleFingerRotation * offset - myRightHandPosition;
					animator.SetIKPosition (AvatarIKGoal.RightHand, targetPosition);
					animator.SetIKPositionWeight (AvatarIKGoal.RightHand, percentComplete);
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
}
