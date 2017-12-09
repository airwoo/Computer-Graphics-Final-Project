using UnityEngine;

public class PlayerController : MonoBehaviour
{
	void Update()
	{
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;


		transform.Translate(x, 0, z);
	}
}

