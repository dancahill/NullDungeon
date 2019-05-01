using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
	GameManager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;
	[HideInInspector] GameObject player;

	private void Awake()
	{
		m_Manager = GameManager.instance;
		m_Animator = GetComponentInChildren<Animator>();
		player = GameObject.Find("Player");
	}

	private void Update()
	{
		if (m_Agent == null || !m_Agent.enabled) return;
		if (m_Agent.hasPath)
		{
			if (m_Agent.remainingDistance < 0.1f)
			{
				m_Agent.ResetPath();
			}
			else if (m_Agent.remainingDistance > 0)
			{
				//Debug.Log("m_Agent.remainingDistance: " + m_Agent.remainingDistance);
			}
		}
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / m_Agent.speed;
		if (m_Animator) m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
	}

	void CreateAgent()
	{
		if (m_Agent) return;
		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		//m_Agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;// not sure if we need this
		m_Agent.autoBraking = false;
		m_Agent.speed = 1.0f;
		if (name == "Zombie") m_Agent.speed = 0.8f;
		m_Agent.angularSpeed = 500;
		//m_Agent.stoppingDistance = 1.1f;
	}

	public void SetDirection()
	{
		Vector3 direction = (player.transform.position - transform.position).normalized;
		Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = lookrotation;
	}

	public void SetDestination(Vector3 v)
	{
		CreateAgent();
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
		//Debug.Log("Combat: " + name + " died");
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

			EnemyModel em = GetComponentInChildren<EnemyModel>();
			if (em) em.EquipSword(false);
		}
		GetComponent<CapsuleCollider>().enabled = false;
	}

	public void Stop()
	{
		//m_Agent.stoppingDistance = 0f;
		//MoveTo(transform.position);
		m_Agent.SetDestination(transform.position);
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

	//private void OnDrawGizmosSelected()
	//{
	//	if (m_Agent == null || !m_Agent.enabled) return;
	//	Gizmos.color = Color.blue;
	//	Gizmos.DrawWireSphere(m_Agent.destination, .1f);
	//}
}
