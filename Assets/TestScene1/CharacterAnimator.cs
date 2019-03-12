using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
	Test1Manager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;
	const float locomotionAnimationSmoothTime = .1f;

	void Start()
	{
		m_Manager = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
		m_Agent = GetComponent<NavMeshAgent>();
		m_Animator = GetComponentInChildren<Animator>();
		//agent.SetDestination(tm.end.transform.position);
	}

	void Update()
	{
		float speedPercent = m_Agent.velocity.magnitude / m_Agent.speed;
		m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		GetInput();
	}

	void GetInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Camera cam = Camera.main;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			LayerMask movementMask = LayerMask.GetMask("Ground");
			if (Physics.Raycast(ray, out hit, movementMask))
			{
				m_Agent.SetDestination(hit.point);
			}
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//m_Manager.CameraZoom = m_Manager.CameraZoom == 0 ? 2 : 0;
			//m_Manager.CameraZoom++;
			if (++m_Manager.CameraZoom > 2) m_Manager.CameraZoom = 0;
		}
	}
}
