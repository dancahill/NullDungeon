using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
	Test1Manager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;

	void Start()
	{
		m_Manager = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
		m_Agent = GetComponent<NavMeshAgent>();
		//m_Animator = GetComponentInChildren<Animator>();
		//agent.SetDestination(tm.end.transform.position);
	}

	void Update()
	{
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / m_Agent.speed;

		// bad code - lets us change avatars at runtime
		m_Animator = GetComponentInChildren<Animator>();
		if (m_Animator)
		{
			m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		}
		GetInput();

		if (m_Manager.SelectedTarget != null)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, m_Manager.SelectedTarget.transform.position);
			if (distanceToEnemy < 1)
			{
				Debug.Log("close! attack?!?");
				m_Manager.SelectedTarget = null;
			}
			else
			{
				Debug.Log("trying to follow " + m_Manager.SelectedTarget.name);
				MoveTo(m_Manager.SelectedTarget.transform.position);
			}
		}
	}

	void GetInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			LayerMask movementMask = LayerMask.GetMask("Ground");
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, movementMask))
			{
				if (hit.transform.name == "Enemy")
				{
					// this should already be handled by the enemy's event
					// maybe this mouse code should be smarter about this
					Debug.Log("hit an enemy?");
				}
				else
				{
					m_Manager.SelectedTarget = null;
					MoveTo(hit.point);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (++m_Manager.CameraZoom > 2) m_Manager.CameraZoom = 0;
		}
	}

	void MoveTo(Vector3 point)
	{
		m_Agent.SetDestination(point);
	}

	public void SetTarget(GameObject target)
	{
		m_Manager.SelectedTarget = target;
		float distanceToEnemy = Vector3.Distance(m_Manager.SelectedTarget.transform.position, m_Manager.player.transform.position);
		Debug.Log("clicked on '" + target.name + "' distance = " + distanceToEnemy.ToString("0.0"));
		MoveTo(m_Manager.SelectedTarget.transform.position);
	}
}
