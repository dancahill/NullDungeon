using UnityEngine;

public class TownNPC : Interactable
{
	public override bool Interact()
	{
		base.Interact();
		switch (name)
		{
			case "Cow":
				GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CowAlright, FindObjectOfType<Player>().Stats.Class);
				return true;
			case "Deckard Cain":
				GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CainHello);
				GameManager.instance.PlayerStats.Life = GameManager.instance.PlayerStats.BaseLife;
				return true;
			default:
				Debug.Log("interaction with " + name + " not defined");
				break;
		}
		return false;
	}
}
