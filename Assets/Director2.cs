using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director2 : MonoBehaviour {
	public GameObject agent;
	public Animator anim;
	public AnimationState a;
	public float ff;
	// Use this for initialization
	void Start () {
		anim = agent.GetComponent<Animator> ();
		anim.speed = 0.0f;

	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ((1.0f/anim.GetCurrentAnimatorClipInfo (0) [0].clip.frameRate).ToString());

		if (Input.GetButtonDown ("Fire1")) {
			float frametime = 1.0f/anim.GetCurrentAnimatorClipInfo (0) [0].clip.frameRate;
			//anim.speed = 1.0f;
			int totalframes = (int)(anim.GetCurrentAnimatorClipInfo (0) [1].clip.length * anim.GetCurrentAnimatorClipInfo (0) [1].clip.frameRate);
			//anim.speed = 0.0f;
			//Debug.Log (totalframes);
			Debug.Log (anim.GetCurrentAnimatorClipInfo (0) [2].clip.name);
			for (int i = 0; i < totalframes; i++) {
				anim.speed = 1.0f;

				anim.Update (frametime);
				anim.speed = 0.0f;
			}
		}

	}
}
