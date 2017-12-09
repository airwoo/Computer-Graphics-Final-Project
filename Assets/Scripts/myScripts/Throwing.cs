using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour {

	public GameObject ballScriptReference;

	public void ThrowBall(){
		ballScriptReference.GetComponent<BallScript> ().ReleaseBall ();
	}
}
