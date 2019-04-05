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
		if (GameManager.GetPlayer().InventoryAdd(item)) Destroy(gameObject);
	}
}
