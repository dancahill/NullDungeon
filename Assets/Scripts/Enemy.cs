﻿using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : Interactable
{
	public static float ProvokeRadius = 5;
	[Header("Other")]
	public Character Stats;
	GameManager m_Manager;
	Scene_Manager m_SceneManager;
	[HideInInspector] public EnemyAnimator Animator;
	[HideInInspector] GameObject player;
	bool Provoked = false;

	Vector3 start;
	Vector3 end;

	void Awake()
	{
		Stats = new Character(Character.CharacterClass.NPC);
		m_Manager = GameManager.instance;
		m_SceneManager = FindObjectOfType<Scene_Manager>();//should probably just use a static instance
		Animator = GetComponent<EnemyAnimator>();
	}

	void Start()
	{
		player = GameObject.Find("Player");
		start = transform.Find("Waypoints/Start").position;
		end = transform.Find("Waypoints/End").position;
	}

	private void Update()
	{
		if (!IsAlive()) return; // dead undead can't attack
		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
		if (distanceToPlayer < ProvokeRadius)
		{
			Provoked = true;
		}
		else
		{
			Provoked = false;
		}
		if (Provoked)
		{
			Animator.SetDestination(player.transform.position);
			if (distanceToPlayer < 1.1)
			{
				Player p = player.GetComponent<Player>();
				if (Stats.CanAttack())
				{
					Animator.DoAttack();
					if (Stats.CalculateDamage(p.Stats, 2.0f, out int damage))
					{
						if (damage > 0) p.TakeDamage(damage);
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
		else
		{
			if (Animator.GetDistance() < 0.5f)
			{
				if (Animator.GetDestination() == start)
				{
					Animator.SetDestination(end);
				}
				else
				{
					Animator.SetDestination(start);
				}
			}
		}
	}

	private void OnMouseOver()
	{
		string s;
		if (Stats.Life <= 0)
			s = string.Format("Dead {0}", name);
		else
			s = string.Format("{0}\nHealth={1}/{2}", name, Stats.Life, Stats.BaseLife);
		FindObjectOfType<GameCanvas>().SetInfo(s);
	}

	//private void OnMouseDown()
	//{
	//	PlayerAnimator PlayerAnimator = player.GetComponent<PlayerAnimator>();
	//	PlayerAnimator.SetTarget(transform.gameObject);
	//}

	//private void OnMouseExit()
	//{
	//	FindObjectOfType<GameCanvas>().SetInfo("");
	//}

	public override bool Interact()
	{
		base.Interact();
		if (!IsAlive()) return false;
		Debug.Log("attacking enemy " + name);
		//Debug.Log("close! attack?!?");
		if (GameManager.instance.PlayerCharacter.CalculateDamage(Stats, 0.5f, out int damage))
		{
			if (damage > 0) TakeDamage(damage);
		}
		else
		{
			//Debug.Log("missed");
		}
		return true;
	}

	public bool IsAlive()
	{
		NavMeshAgent m_Agent = GetComponent<NavMeshAgent>();
		if (Stats.Life <= 0 && m_Agent.enabled)
		{
			Debug.Log("enemy " + name + " should already be dead");
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
		Debug.Log(name + " takes " + damage + " damage");
		if (Stats.Life <= 0)
		{
			Stats.Life = 0;
			GetComponent<EnemyAnimator>().Death();
			m_Manager.PlayerCharacter.AddExperience(Stats.GivesExperience);
		}
		else
		{
			if (name == "Skeleton")
				m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.SkeletonHit1);
			else
				m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ZombieHit1);
		}
	}
}
