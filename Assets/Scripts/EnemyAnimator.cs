using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
	GameManager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;
	Vector3 start;
	Vector3 end;
	bool movingtoend;

	void Start()
	{
		m_Manager = GameManager.instance;
		m_Animator = GetComponentInChildren<Animator>();

		start = transform.Find("Waypoints/Start").position;
		end = transform.Find("Waypoints/End").position;
		movingtoend = true;

		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		m_Agent.speed = 2.0f;
		if (name == "Zombie") m_Agent.speed = 1.5f;
		m_Agent.angularSpeed = 500;
		m_Agent.SetDestination(end);
	}

	private void Update()
	{
		if (m_Agent == null || !m_Agent.enabled) return;
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / m_Agent.speed;

		if (m_Animator) m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		if (m_Agent.remainingDistance < 0.5f)
		{
			movingtoend = !movingtoend;
			m_Agent.SetDestination(movingtoend ? end : start);
		}
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
