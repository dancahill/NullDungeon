﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerAnimator : MonoBehaviour
{
	GameManager m_Manager;
	Player player;
	NavMeshAgent m_Agent;
	Animator m_Animator;
	float maxspeed = 3.0f;
	GameObject SelectedTarget = null;

	void Start()
	{
		m_Manager = GameManager.instance;
		player = gameObject.GetComponentInParent<Player>();
		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		m_Agent.speed = maxspeed;
		m_Agent.angularSpeed = 500;
		if (SceneManager.GetActiveScene().name == "Town")
		{
			m_Agent.speed = 2.0f;
		}
	}

	void Update()
	{
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / maxspeed;
		m_Animator = GetComponentInChildren<Animator>();
		if (m_Animator) m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		//float x = m_Animator.GetFloat("SpeedPercent");
		//Debug.Log(string.Format("{0} {1}", speedPercent, x));
		if (SelectedTarget != null)
		{
			float distanceToTarget = Vector3.Distance(transform.position, SelectedTarget.transform.position);
			//Debug.Log("distance to target is " + distanceToTarget);
			if (distanceToTarget < 1.2)
			{
				bool targetresolved = false;
				TownNPC t = SelectedTarget.gameObject.GetComponent<TownNPC>();
				if (t)
				{
					targetresolved = true;
					Stop();
					t.Interact();
				}
				Enemy e = SelectedTarget.gameObject.GetComponent<Enemy>();
				if (e)
				{
					targetresolved = true;
					if (e.IsAlive())
					{
						//Debug.Log("close! attack?!?");
						if (player.Stats.CanAttack())
						{
							DoAttack();
							int damage;
							if (player.Stats.CalculateDamage(e.Stats, 0.5f, out damage))
							{
								if (damage > 0) e.TakeDamage(damage);
							}
							else
							{
								//Debug.Log("missed");
							}
						}
						else
						{
							//Debug.Log("cooling down");
						}
					}
				}
				SelectedTarget = null;
				if (!targetresolved)
				{
					Debug.Log("target not resolved");
				}
			}
			else
			{
				MoveTo(SelectedTarget.transform.position);
			}
		}
		// adding a rigidbody to player caused some really weird and broken
		// movement, but we need it to trigger waypoints. this seems to fix it.
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}

	void MoveTo(Vector3 point)
	{
		m_Agent.SetDestination(point);
	}

	public void SetTarget(GameObject target)
	{
		SelectedTarget = target;
		float distanceToTarget = Vector3.Distance(SelectedTarget.transform.position, Scene_Manager.instance.player.transform.position);
		//Debug.Log("clicked on '" + target.name + "' distance = " + distanceToEnemy.ToString("0.0"));
		//Debug.Log("distance to target is " + distanceToTarget);
		if (distanceToTarget > 1.2)
		{
			MoveTo(SelectedTarget.transform.position);
		}
		else
		{
			player.Animator.Stop();
			player.Animator.SetDirection();
		}
	}

	/// <summary>
	/// doesn't move - just turns the player to face that direction
	/// </summary>
	public void SetDirection()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask movementMask = LayerMask.GetMask("Ground");
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, movementMask))
		{
			// looks ugly turning this suddenly, but good enough for now
			transform.LookAt(hit.point);
		}
	}

	/// <summary>
	/// try to move the player to this location
	/// </summary>
	public void SetDestination()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask movementMask = LayerMask.GetMask("Ground");
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, movementMask))
		{
			string hitname = hit.transform.name;
			//if (hit.transform.name.StartsWith("Floor"))
			//if (hit.transform.gameObject.layer == movementMask)
			//if ((movementMask & 1 << hit.transform.gameObject.layer) == 1 << hit.transform.gameObject.layer)
			// this is ugly - find a better solution
			if (hitname.StartsWith("Floor") || hitname.StartsWith("Stairs") || hitname == "Plane" || hitname == "Terrain" || hitname == "Start" || hitname == "End")
			{
				SelectedTarget = null;
				MoveTo(hit.point);
			}
		}
	}

	public void Stop()
	{
		MoveTo(transform.position);
	}

	/// <summary>
	/// play the attack animation and sound
	/// </summary>
	public void DoAttack()
	{
		// this should probably time these things. maybe use an animator
		m_Animator.Play("Attack");
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.PlayerAttack1, FindObjectOfType<Player>().Stats.Class);
	}
}
