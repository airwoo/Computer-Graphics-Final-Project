using UnityEngine;

public class GrabSMB : StateMachineBehaviour
{

	private readonly int iG = Animator.StringToHash("isGrab");

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool (iG, false);

	}
}