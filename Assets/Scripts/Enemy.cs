using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	GameManager m_Manager;
	public NavMeshAgent m_Agent;
	const float starthealth = 100;
	float health;

	void Start()
	{
		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		m_Agent.speed = 2.0f;
		if (name == "Zombie") m_Agent.speed = 1.5f;
		m_Agent.angularSpeed = 500;
		m_Manager = GameManager.instance;
		health = starthealth;
	}

	private void OnMouseDown()
	{
		m_Manager.PlayerAnimator.SetTarget(transform.gameObject);
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		Debug.Log(name + " takes " + damage + " damage");
		if (health <= 0)
		{
			GetComponent<EnemyAnimator>().Death();
		}
	}
}
