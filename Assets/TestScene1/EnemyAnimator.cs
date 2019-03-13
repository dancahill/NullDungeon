using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
	GameManager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;

	GameObject start;
	GameObject end;
	bool movingtoend;

	void Start()
	{
		//m_Manager = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
		m_Manager = GameManager.instance;
		m_Agent = GetComponent<NavMeshAgent>();
		m_Animator = GetComponentInChildren<Animator>();

		start = GameObject.Find("Start");
		end = GameObject.Find("End");

		movingtoend = true;
		m_Agent.SetDestination(end.transform.position);
	}

	private void Update()
	{
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / m_Agent.speed;

		if (m_Animator)
		{
			m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		}
		if (m_Agent.remainingDistance < 0.5f)
		{
			if (movingtoend)
			{
				movingtoend = false;
				m_Agent.SetDestination(start.transform.position);
			}
			else
			{
				movingtoend = true;
				m_Agent.SetDestination(end.transform.position);
			}
		}
	}
}
