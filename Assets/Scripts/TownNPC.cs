using UnityEngine;

public class TownNPC : MonoBehaviour
{
	void OnMouseOver()
	{
		FindObjectOfType<GameCanvas>().SetInfo(name);
	}

	void OnMouseExit()
	{
		FindObjectOfType<GameCanvas>().SetInfo("");
	}

	void OnMouseDown()
	{
		Scene_Manager.instance.PlayerAnimator.SetTarget(transform.gameObject);
	}

	public void Interact()
	{
		switch (name)
		{
			case "Cow":
				GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CowAlright, FindObjectOfType<Player>().Stats.Class);
				break;
			case "Deckard Cain":
				GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CainHello);
				GameManager.instance.PlayerStats.Life = GameManager.instance.PlayerStats.BaseLife;
				break;
			default:
				Debug.Log("interaction with " + name + " not defined");
				break;
		}
	}
}
