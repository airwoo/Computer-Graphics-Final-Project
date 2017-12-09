using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class animationControls : MonoBehaviour {
	Animator anim;
	NavMeshAgent agent;
	Vector2 smoothDelta = Vector2.zero;
	Vector2 veloc = Vector2.zero;

	private readonly int Hor = Animator.StringToHash ("Horizontal");
	private readonly int Ver = Animator.StringToHash ("Vertical");
	private readonly int m_blender = Animator.StringToHash("Blend");
	private readonly int jumping = Animator.StringToHash("isJumping");
	private float blend;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.updatePosition = false;
		blend = 0.0f;
	
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(KeyCode.LeftShift) == true){
			if (blend < 0.01f) {
				blend = 1.0f;
				agent.speed = 3.5f;
			} else {
				blend = 0.0f;
				agent.speed = 1.0f;
			}
		}
		if (agent.isOnOffMeshLink) {
			anim.Play ("Jumping");
		} else {
			anim.SetBool (jumping, false);
		}
		anim.SetFloat (m_blender, blend, 0.15f, Time.deltaTime);
 
		Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

		float dx = Vector3.Dot (transform.right, worldDeltaPosition);
		float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
		Vector2 deltaPos = new Vector2 (dx, dy);

		float smooth = Mathf.Min (1.0f, Time.deltaTime / 0.15f);
		smoothDelta = Vector2.Lerp (smoothDelta, deltaPos, smooth);

		if (Time.deltaTime > 1e-5f)
			veloc = smoothDelta / Time.deltaTime;

		bool moving = veloc.magnitude > 0.5f && agent.remainingDistance > agent.radius;

		anim.SetFloat (Hor, veloc.x);
		anim.SetFloat (Ver, veloc.y);


	}
	void OnAnimatorMove()
	{
		transform.position = agent.nextPosition;
	}
	IEnumerator Doanimation()
	{
		
		yield return new WaitForSeconds (3.0f);
	}

}
