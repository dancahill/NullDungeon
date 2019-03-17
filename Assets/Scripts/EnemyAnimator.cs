using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
	GameManager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;

	void Start()
	{
		m_Manager = GameManager.instance;
		m_Animator = GetComponentInChildren<Animator>();


		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		m_Agent.speed = 2.0f;
		if (name == "Zombie") m_Agent.speed = 1.5f;
		m_Agent.angularSpeed = 500;
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
		return m_Agent.destination;
	}
	public float GetDistance()
	{
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
		else
			m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ZombieDie1);
		Debug.Log(name + " died");
		m_Agent.speed = 0;
		m_Animator.Play("Death");
		yield return new WaitForSeconds(2);
		//Destroy(gameObject);
		// maybe we can leave it there for decoration
		m_Agent.enabled = false;
		CapsuleCollider cc = GetComponent<CapsuleCollider>();
		cc.enabled = false;
	}
}
