﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
	Test1Manager m_Manager;
	const float starthealth = 100;
	float health;

	void Start()
	{
		m_Manager = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
		health = 100;
	}

	private void OnMouseDown()
	{
		m_Manager.PlayerAnimator.SetTarget(transform.gameObject);
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Debug.Log("skelly died");
			gameObject.SetActive(false);
		}
		else
		{
			Debug.Log("skelly takes " + damage + " damage");
		}
	}
}