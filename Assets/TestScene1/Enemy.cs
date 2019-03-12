using UnityEngine;

public class Enemy : MonoBehaviour
{
	Test1Manager m_Manager;

	void Start()
	{
		m_Manager = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
	}

	private void OnMouseDown()
	{
		m_Manager.PlayerAnimator.SetTarget(transform.gameObject);
	}
}
