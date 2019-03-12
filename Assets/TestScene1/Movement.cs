/*
using UnityEngine;

public class Movement : MonoBehaviour
{
	public GameObject m_Character;
	public GameObject m_Camera;
	public float moveSpeed = 10f;
	public float turnSpeed = 1f;

	void Update()
	{
		float movefast = (Input.GetKey(KeyCode.LeftShift) ? 2 : 1);

		if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
		{
			m_Character.transform.Translate(Vector3.forward * moveSpeed * movefast * Time.deltaTime, Space.Self);
		}
		if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
		{
			m_Character.transform.Translate(Vector3.back * moveSpeed * movefast * Time.deltaTime, Space.Self);
		}
		if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 rotateValue = new Vector3(0, -turnSpeed * movefast, 0);
			m_Character.transform.eulerAngles = m_Character.transform.eulerAngles - rotateValue;
		}
		if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 rotateValue = new Vector3(0, turnSpeed * movefast, 0);
			m_Character.transform.eulerAngles = m_Character.transform.eulerAngles - rotateValue;
		}
	}
}
*/
