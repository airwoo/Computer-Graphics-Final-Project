using UnityEngine;

public class LocomotionSMB : StateMachineBehaviour
{
	public float m_Damping = 0.15f;

	private readonly int m_HashHorizontalPara = Animator.StringToHash ("Horizontal");
	private readonly int m_HashVerticalPara = Animator.StringToHash ("Vertical");
	private readonly int m_blender = Animator.StringToHash("Blend");
	private readonly int iJ = Animator.StringToHash("isJumping");
	private readonly int iG = Animator.StringToHash("isGrab");
	//private readonly int iC = Animator.StringToHash("isClamped");
	private readonly int iCr = Animator.StringToHash("isCrouch");
	private float blend = 0.0f;


	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Input.GetKeyDown (KeyCode.LeftControl) == true) {
			bool temp = animator.GetBool (iCr);
			animator.SetBool (iCr, !temp);
			return;
		}
		if (Input.GetKeyDown ("space") == true) {
			animator.SetBool (iJ, true);
			return;
		}
		if (Input.GetKeyDown ("g") == true) {
			
		}

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		if(Input.GetKeyDown(KeyCode.LeftShift) == true){
			if (blend < 0.01f) {
				blend = 1.0f;
			} else {
				blend = 0.0f;
			}
		}
		Vector2 input = new Vector2(horizontal, vertical).normalized;

		animator.SetFloat(m_HashHorizontalPara, input.x, m_Damping, Time.deltaTime);
		animator.SetFloat(m_HashVerticalPara, input.y, m_Damping, Time.deltaTime);
		animator.SetFloat (m_blender, blend, m_Damping, Time.deltaTime);

	}
}