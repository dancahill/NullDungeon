using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
	public CharacterStats Stats;
	GameManager m_Manager;
	[HideInInspector] public EnemyAnimator Animator;
	[HideInInspector] GameObject player;
	bool Provoked = false;

	Vector3 start;
	Vector3 end;

	void Start()
	{
		Stats = new CharacterStats(CharacterStats.CharacterClass.NPC);
		m_Manager = GameManager.instance;
		Animator = GetComponent<EnemyAnimator>();
		player = GameObject.Find("Player");

		start = transform.Find("Waypoints/Start").position;
		end = transform.Find("Waypoints/End").position;
	}

	private void Update()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
		if (distanceToPlayer < 5)
		{
			Provoked = true;
		}
		if (Provoked)
		{
			Animator.SetDestination(player.transform.position);
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
		{
			s = string.Format("Dead {0}", name);
		}
		else
		{
			s = string.Format("{0}\nHealth={1}/{2}", name, Stats.Life, Stats.BaseLife);
		}
		FindObjectOfType<GameCanvas>().SetInfo(s);
	}

	private void OnMouseDown()
	{
		m_Manager.PlayerAnimator.SetTarget(transform.gameObject);
	}

	private void OnMouseExit()
	{
		FindObjectOfType<GameCanvas>().SetInfo("");
	}

	public bool IsAlive()
	{
		return (Stats.Life > 0);
	}

	public void TakeDamage(int damage)
	{
		if (Stats.Life <= 0)
		{
			Debug.Log(name + " is dead, jim");
			return;
		}
		Stats.Life -= damage;
		Debug.Log(name + " takes " + damage + " damage");
		if (Stats.Life <= 0)
		{
			Stats.Life = 0;
			GetComponent<EnemyAnimator>().Death();
			m_Manager.player.GetComponent<Player>().Stats.AddExperience(Stats.GivesExperience);
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
