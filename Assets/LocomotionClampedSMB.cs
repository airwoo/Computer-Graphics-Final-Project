using UnityEngine;

public class LocomotionClampedSMB : StateMachineBehaviour
{
	public float m_Damping = 0.15f;


	private readonly int m_HashHorizontalPara = Animator.StringToHash ("Horizontal");
	private readonly int m_HashVerticalPara = Animator.StringToHash ("Vertical");
	private readonly int iC = Animator.StringToHash("isClamped");

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Input.GetKeyDown (KeyCode.LeftControl) == true) {
			animator.SetBool (iC, false);
			return;
		}

		float horizontal = Input.GetAxis ("Horizontal2");
		float vertical = 0.0f;

		Vector2 input = new Vector2(horizontal, vertical).normalized;

		animator.SetFloat(m_HashHorizontalPara, input.x, m_Damping, Time.deltaTime);
		animator.SetFloat(m_HashVerticalPara, input.y, m_Damping, Time.deltaTime);

	}
}