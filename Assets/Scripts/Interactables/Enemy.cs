﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : Interactable
{
	public Image healthBar;
	public static float ProvokeRadius = 5;
	[Header("Other")]
	public Character Stats;
	GameManager m_Manager;
	SceneManager m_SceneManager;
	[HideInInspector] public EnemyAnimator Animator;
	[HideInInspector] GameObject player;
	bool Activated = false;
	bool Provoked = false;

	Vector3 start;
	Vector3 end;
	Vector3 destination;

	protected override void Awake()
	{
		Stats = new Character(Character.CharacterClass.NPC, name);
		m_Manager = GameManager.instance;
		m_SceneManager = FindObjectOfType<SceneManager>();//should probably just use a static instance
		Animator = GetComponent<EnemyAnimator>();
		if (overheadCanvas) overheadCanvas.gameObject.SetActive(false);
	}

	void Start()
	{
		player = GameObject.Find("Player");
		start = transform.Find("Waypoints/Start").position;
		end = transform.Find("Waypoints/End").position;
		destination = end;

		EnemyModel em = GetComponentInChildren<EnemyModel>();
		if (em) em.EquipSword(true);
	}

	protected override void Update()
	{
		base.Update();
		if (!IsAlive()) return; // dead undead can't attack
		if (overheadCanvas && overheadCanvas.enabled)
		{
			healthBar.fillAmount = Stats.Life / Stats.BaseLife;
		}
		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
		//if (!Activated && distanceToPlayer < ProvokeRadius * 2) Activated = true;
		Activated = (distanceToPlayer < ProvokeRadius * 2);
		Provoked = (distanceToPlayer < ProvokeRadius);
		if (Provoked)
		{
			if (distanceToPlayer < 1.1)
			{
				Animator.SetDirection();
				if (Stats.CanAttack())
				{
					Animator.Stop();
					Animator.DoAttack();
					Player p = player.GetComponent<Player>();
					if (Stats.CalculateDamage(p.Stats, out int damage))
					{
						if (damage > 0) p.TakeDamage(damage);
					}
					else
					{
						//Debug.Log("Combat: missed");
					}
				}
				else
				{
					//Debug.Log("Combat: cooling down");
				}
			}
			else
			{
				Animator.SetDestination(player.transform.position);
			}
		}
		else if (Activated)
		{
			//if (Animator.GetDistance() < 0.5f)
			//{
			//	if (Animator.GetDestination() == start)
			//	{
			//		Animator.SetDestination(end);
			//	}
			//	else
			//	{
			//		Animator.SetDestination(start);
			//	}
			//}
			float distanceToWP = Vector3.Distance(transform.position, destination);
			if (distanceToWP < 0.5f)
			{
				destination = (destination == start) ? end : start;
			}
			if (Animator.GetDestination() != destination) Animator.SetDestination(destination);
		}
	}

	//private void OnMouseOver()
	//{
	//	Debug.Log("enemy mouse over " + name);
	//	EnableCanvas();
	//	string s;
	//	if (Stats.Life <= 0)
	//		s = string.Format("Dead {0}", name);
	//	else
	//		s = string.Format("{0}\nHealth={1}/{2}", name, Stats.Life, Stats.BaseLife);
	//	FindObjectOfType<GameCanvas>().SetInfo(s);
	//}

	public override bool Interact()
	{
		base.Interact();
		if (!IsAlive()) return false;
		//Debug.Log("Combat: attacking enemy " + name);
		//Debug.Log("Combat: close! attack?!?");
		if (GameManager.instance.PlayerCharacter.CalculateDamage(Stats, out int damage))
		{
			if (damage > 0) TakeDamage(damage);
		}
		else
		{
			//Debug.Log("Combat: missed");
		}
		return true;
	}

	public bool IsAlive()
	{
		NavMeshAgent m_Agent = GetComponent<NavMeshAgent>();
		if (Stats.Life <= 0 && m_Agent && m_Agent.enabled)
		{
			Debug.Log("Combat: enemy " + name + " should already be dead");
			MakeDead();
		}
		return (Stats.Life > 0);
	}

	public void MakeDead()
	{
		Stats.Life = 0;
		GetComponent<EnemyAnimator>().SetDead(true);
	}

	public void TakeDamage(int damage)
	{
		if (damage <= 0 || Stats.Life <= 0) return;
		Stats.Life -= damage;
		//Debug.Log("Combat: " + name + " takes " + damage + " damage");
		if (Stats.Life <= 0)
		{
			Stats.Life = 0;
			GetComponent<EnemyAnimator>().Death();
			Character p = m_Manager.PlayerCharacter;
			if (p.Level < Stats.Level + 10)
			{
				float exp = (float)Stats.GivesExperience * (1.0f + 0.1f * (float)(Stats.Level - p.Level));
				//Debug.Log("Combat: adding " + exp + " experience");
				p.AddExperience((int)exp);
			}
			else
			{
				Debug.Log("Combat: level too low - no experience");
			}
			FindObjectOfType<LootManager>().DropRandom(transform.position, 1);
		}
		else
		{
			if (name == "Skeleton")
				m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.SkeletonHit1);
			else
				m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ZombieHit1);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!IsAlive()) return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, ProvokeRadius);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, ProvokeRadius * 2);
		//if (IsAlive())
		//{
		//	Gizmos.color = Color.green;
		//	Gizmos.DrawWireSphere(start, .1f);
		//	Gizmos.color = Color.red;
		//	Gizmos.DrawWireSphere(end, .1f);
		//}
	}
}
