using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
	public Character Stats;
	[HideInInspector] public GameManager Manager;
	[HideInInspector] public PlayerAnimator Animator;

	[HideInInspector] public PlayerModel model;

	GameObject warrior;
	GameObject rogue;

	private void Start()
	{
		Manager = GameManager.instance;
		Stats = GameManager.instance.PlayerCharacter;
		Animator = GetComponent<PlayerAnimator>();
		model = GetComponentInChildren<PlayerModel>();

		warrior = transform.Find("Warrior").gameObject;
		rogue = transform.Find("Rogue").gameObject;

		if (SceneController.GetActiveSceneName() == "Town")
		{
			GetComponentInChildren<Light>().enabled = false;
		}
	}

	private void Update()
	{
		warrior.SetActive(Stats.Class == Character.CharacterClass.Warrior);
		rogue.SetActive(Stats.Class == Character.CharacterClass.Rogue);
		if (Stats.Life > 0)
		{
			// five minutes to heal fully. more base health means you heal faster
			//Stats.Life = Mathf.Clamp(Stats.Life + ((float)Stats.BaseLife / 300f * Time.deltaTime), 0, Stats.BaseLife);
			//Stats.Mana = Mathf.Clamp(Stats.Mana + ((float)Stats.BaseLife / 300f * Time.deltaTime), 0, Stats.BaseMana);

			float speed = Animator.GetSpeed();
			if (speed > 0.1f && Stats.CanStep(speed)) // this needs work on the timing
			{
				//Debug.Log("step '" + speed + "'");
				//SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Footstep1);
			}
			model.EquipSword(Stats.Equipped.righthand != null);
			model.EquipShield(Stats.Equipped.lefthand != null);
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
