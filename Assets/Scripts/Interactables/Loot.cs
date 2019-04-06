public class Loot : Interactable
{
	public Item item;

	protected override void Update()
	{
		base.Update();
		if (GameManager.instance.Settings.ShowItemsOnGround)
		{
			EnableCanvas();
		}
		else
		{
			DisableCanvas();
		}
	}

	public override bool Interact()
	{
		base.Interact();
		PickUp();
		return false;
	}

	void PickUp()
	{
		if (GameManager.GetPlayer().InventoryAdd(item))
		{
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Click);
			Destroy(gameObject);
		}
	}
}
