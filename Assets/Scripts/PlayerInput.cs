﻿using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
	GameManager m_Manager;
	Player player;

	private void Start()
	{
		m_Manager = GameManager.instance;
		player = gameObject.GetComponentInParent<Player>();
	}

	private void Update()
	{
		GetInput();
	}

	void GetInput()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			//Debug.Log("Clicked on the UI");
		}
		else
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (Input.GetMouseButtonDown(0))
				{
					player.Animator.Stop();
					player.Animator.SetDirection();
					player.Animator.DoAttack();
				}
			}
			else
			{
				if (Input.GetMouseButton(0))
				{
					player.Animator.SetDestination();
				}
			}
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			// fucking idiots. sign of zero should return zero, not one
			if (scroll != 0) m_Manager.Settings.CameraZoom += Mathf.Sign(scroll);
			m_Manager.Settings.CameraZoom = Mathf.Clamp(m_Manager.Settings.CameraZoom, 0, 2);
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			m_Manager.Canvas.OpenCharacterPanel();
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			m_Manager.Canvas.OpenInventoryPanel();
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (++m_Manager.Settings.CameraZoom > 2) m_Manager.Settings.CameraZoom = 0;
		}
	}
}