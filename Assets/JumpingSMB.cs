using UnityEngine;

public class JumpingSMB : StateMachineBehaviour
{

	private readonly int iJ = Animator.StringToHash("isJumping");

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool (iJ, false);

	}
}