using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
	GameManager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;

	private void Awake()
	{
		// this causes warnings in awake but if put in start, its late load causes errors in dungeon restore
	}

	void Start()
	{
		m_Manager = GameManager.instance;
		m_Animator = GetComponentInChildren<Animator>();
		m_Agent = gameObject.AddComponent<NavMeshAgent>();

		m_Agent.speed = 1.0f;
		if (name == "Zombie") m_Agent.speed = 0.8f;
		m_Agent.angularSpeed = 500;
		m_Agent.stoppingDistance = 1.1f;
	}

	private void Update()
	{
		if (m_Agent == null || !m_Agent.enabled) return;
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / m_Agent.speed;

		if (m_Animator) m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
	}

	public void SetDestination(Vector3 v)
	{
		if (m_Agent == null || !m_Agent.enabled) return;
		m_Agent.SetDestination(v);
	}

	public Vector3 GetDestination()
	{
		if (m_Agent == null || !m_Agent.enabled) return transform.position;
		return m_Agent.destination;
	}
	public float GetDistance()
	{
		if (m_Agent == null || !m_Agent.enabled) return 0;
		return m_Agent.remainingDistance;
	}

	public void Death()
	{
		StartCoroutine(DoDeath());
	}

	IEnumerator DoDeath()
	{
		if (name == "Skeleton")
			m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.SkeletonDie1);
		else if (name == "Zombie")
			m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ZombieDie1);
		Debug.Log(name + " died");
		SetDead(false);
		m_Animator.Play("Death");
		yield return new WaitForSeconds(2);
	}

	public void SetDead(bool alreadydead)
	{
		//Destroy(gameObject);
		// maybe we can leave it there for decoration
		//if (m_Agent)
		//{
		m_Agent.speed = 0;
		m_Agent.enabled = false;
		//		}

		if (alreadydead)
		{
			m_Animator.Play("RestoreDeath");
		}

		if (name == "Skeleton")
		{
			transform.position -= new Vector3(0, 0.85f, 0);
		}
		GetComponent<CapsuleCollider>().enabled = false;
	}


	/// <summary>
	/// play the attack animation and sound
	/// </summary>
	public void DoAttack()
	{
		// this should probably time these things. maybe use an animator
		m_Animator.Play("Attack");
		if (name == "Skeleton")
			m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.SkeletonAttack1);
		else if (name == "Zombie")
			m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ZombieAttack1);
	}
}
