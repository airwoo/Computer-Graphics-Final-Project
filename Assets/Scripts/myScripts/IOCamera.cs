using UnityEngine;
using System.Collections;

public class IOCamera: MonoBehaviour {

	public float turnSpeed = 4.0f;
	public Transform player;

	private Vector3 offset, rightoff;

	void Start () {
		offset = new Vector3(player.position.x, player.position.y + 4.0f, player.position.z + 3.5f);
	}

	void LateUpdate()
	{
		//offset = Quaternion.AngleAxis (Input.mousePosition.x * turnSpeed, Vector3.up) * offset;
		float horizontal = Input.GetAxis("Mouse X") * turnSpeed;
		float vertical = Input.GetAxis ("Mouse Y") * turnSpeed;
		player.Rotate (0, horizontal, 0);

		float desiredAngle = player.transform.eulerAngles.y;

		rightoff = Vector3.Cross (Vector3.up, offset);
		Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);
		transform.position = player.transform.position - (rotation * offset);
		//offset = Quaternion.AngleAxis (Input.mousePosition.y*-1.0f *turnSpeed, rightoff) * offset;
		//offset = offset * Input.GetAxis ("Mouse ScrollWheel");
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			offset = offset / 1.1f;
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			offset = offset * 1.1f;
		}
		//transform.position = player.position + offset; 
		transform.LookAt(player.position);
	}
}