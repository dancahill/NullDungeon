using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
	public CharacterStats Stats;
	[HideInInspector] public GameManager Manager;
	[HideInInspector] public PlayerAnimator Animator;
	[HideInInspector] public PlayerInput Input;

	private void Start()
	{
		Stats = new CharacterStats(CharacterStats.CharacterClass.Warrior);
		Manager = GameManager.instance;
		Animator = GetComponent<PlayerAnimator>();
		Input = GetComponent<PlayerInput>();
	}

	public void TakeDamage(int damage)
	{
		if (damage <= 0 || Stats.Life <= 0) return;
		Stats.Life -= damage;
		Debug.Log(name + " takes " + damage + " damage");
		if (Stats.Life <= 0)
		{
			Stats.Life = 0;
			//GetComponent<EnemyAnimator>().Death();
			//m_Manager.player.GetComponent<Player>().Stats.AddExperience(Stats.GivesExperience);
			FindObjectOfType<GameCanvas>().SetInfo("You died");
			Manager.m_SoundManager.PlaySound(SoundManager.Sounds.PlayerDie1);
			//Time.timeScale = 0f;
		}
		else
		{
			Manager.m_SoundManager.PlaySound(SoundManager.Sounds.PlayerHit1);
		}
	}
}
