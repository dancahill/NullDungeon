using UnityEngine;
using UnityEngine.UI;

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

		if (Stats.Class == Character.CharacterClass.NPC) Stats.Class = Character.CharacterClass.Warrior;
		SetupMiniMap();
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

			//float speed = Animator.GetSpeed();
			//if (speed > 0.1f && Stats.CanStep(speed)) // this needs work on the timing
			//{
			//Debug.Log("step '" + speed + "'");
			//SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Footstep1);
			//}
			model.EquipSword(Stats.Equipped.righthand != null);
			model.EquipShield(Stats.Equipped.lefthand != null);
		}
	}

	public void TakeDamage(int damage)
	{
		if (damage <= 0 || Stats.Life <= 0) return;
		Stats.Life -= damage;
		//Debug.Log("Combat: " + name + " takes " + damage + " damage");
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

	public void SetupMiniMap()
	{
		GameObject mapimage = GameObject.Find("Canvas/Game/Minimap Image");
		GameObject mapcamera = GameObject.Find("Player/Minimap Camera");
		if (!mapimage || !mapcamera)
		{
			Debug.LogWarning("minimap object not found");
			return;
		}
		RawImage mimage = mapimage.GetComponent<RawImage>();
		Camera mcamera = mapcamera.GetComponent<Camera>();
		if (!mimage || !mcamera)
		{
			Debug.LogWarning("minimap raw image or camera not found");
			return;
		}
		mcamera.targetTexture = (RenderTexture)mimage.texture;
	}
}
