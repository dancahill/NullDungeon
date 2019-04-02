using UnityEngine;

public class Stash : Interactable
{
	public override bool Interact()
	{
		base.Interact();
		GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CantDoThat, FindObjectOfType<Player>().Stats.Class);
		return false;
	}
}
