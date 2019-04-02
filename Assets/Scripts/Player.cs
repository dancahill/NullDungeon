using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
	public Character Stats;
	[HideInInspector] public GameManager Manager;
	[HideInInspector] public PlayerAnimator Animator;
	[HideInInspector] public PlayerInput Input;

	private void Start()
	{
		Manager = GameManager.instance;
		Stats = GameManager.instance.PlayerCharacter;
		Animator = GetComponent<PlayerAnimator>();
		Input = GetComponent<PlayerInput>();

		void setactive(string cclass)
		{
			GameObject warrior = transform.Find("Warrior").gameObject;
			GameObject rogue = transform.Find("Rogue").gameObject;
			warrior.SetActive(cclass == "Warrior");
			rogue.SetActive(cclass == "Rogue");
		}
		if (Stats.Class == Character.CharacterClass.Warrior)
		{
			setactive("Warrior");
		}
		else if (Stats.Class == Character.CharacterClass.Rogue)
		{
			setactive("Rogue");
		}
		if (SceneController.GetActiveSceneName() == "Town")
		{
			GetComponentInChildren<Light>().enabled = false;
		}
	}

	private void Update()
	{
		if (Stats.Life > 0)
		{
			// three minutes to heal fully. more base health means you heal faster
			Stats.Life = Mathf.Clamp(Stats.Life + ((float)Stats.BaseLife / 180f * Time.deltaTime), 0, Stats.BaseLife);
			Stats.Mana = Mathf.Clamp(Stats.Mana + ((float)Stats.BaseLife / 180f * Time.deltaTime), 0, Stats.BaseMana);
		}
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
			Manager.m_SoundManager.PlaySound(SoundManager.Sounds.PlayerDie1, FindObjectOfType<Player>().Stats.Class);
			Manager.Settings.NewInTown = true;
			Character stats = Manager.PlayerCharacter;
			GameSave.SaveCharacter();
			Manager.sceneController.FadeAndLoadScene("Town");
		}
		else
		{
			Manager.m_SoundManager.PlaySound(SoundManager.Sounds.PlayerHit1, FindObjectOfType<Player>().Stats.Class);
		}
	}
}
