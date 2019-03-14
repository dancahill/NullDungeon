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

	private void OnMouseOver()
	{
		string s;
		if (health <= 0)
		{
			s = string.Format("Dead {0}", name);
		}
		else
		{
			s = string.Format("{0}\nHealth={1}/{2}", name, health, starthealth);
		}
		FindObjectOfType<GameCanvas>().SetInfo(s);
	}

	private void OnMouseDown()
	{
		m_Manager.PlayerAnimator.SetTarget(transform.gameObject);
	}

	private void OnMouseExit()
	{
		FindObjectOfType<GameCanvas>().SetInfo("");
	}

	public bool IsAlive()
	{
		return (health > 0);
	}

	public void TakeDamage(float damage)
	{
		if (health <= 0)
		{
			Debug.Log(name + " is dead, jim");
			return;
		}
		health -= damage;
		Debug.Log(name + " takes " + damage + " damage");
		if (health <= 0)
		{
			GetComponent<EnemyAnimator>().Death();
		}
		else
		{
			if (name == "Skeleton")
				m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.SkeletonHit1);
			else
				m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ZombieHit1);
		}
	}
}
