﻿using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
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
				m_Agent.SetDestination(hit.point);
			}
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (++m_Manager.CameraZoom > 2) m_Manager.CameraZoom = 0;
		}
	}
}
