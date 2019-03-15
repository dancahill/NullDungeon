using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
	public CharacterStats Stats;
	GameManager m_Manager;

	void Start()
	{
		Stats = new CharacterStats(CharacterStats.CharacterClass.NPC);
		m_Manager = GameManager.instance;
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
			s = string.Format("{0}\nHealth={1}/{2}", name, Stats.Life, Stats.MaxLife);
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
			m_Manager.player.GetComponent<Player>().Stats.Experience += Stats.GivesExperience;
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
