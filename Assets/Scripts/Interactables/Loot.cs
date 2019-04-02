using UnityEngine;

public class Loot : Interactable
{
	public Item item;

	public override bool Interact()
	{
		base.Interact();
		PickUp();
		return false;
	}

	void PickUp()
	{
		bool added = GameManager.instance.PlayerCharacter.InventoryAdd(item);
		if (added) Destroy(gameObject);
	}
}
