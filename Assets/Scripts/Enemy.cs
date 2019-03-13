using UnityEngine;

public class Enemy : MonoBehaviour
{
	GameManager m_Manager;
	const float starthealth = 100;
	float health;

	void Start()
	{
		//m_Manager = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
		m_Manager = GameManager.instance;
		health = 100;
	}

	private void OnMouseDown()
	{
		m_Manager.PlayerAnimator.SetTarget(transform.gameObject);
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Debug.Log(name + " died");
			gameObject.SetActive(false);
		}
		else
		{
			Debug.Log(name + " takes " + damage + " damage");
		}
	}
}
