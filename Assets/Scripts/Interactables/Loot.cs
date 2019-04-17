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
		if (GameManager.GetPlayer().InventoryAdd(item))
		{
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Click);
			Destroy(gameObject);
		}
		else
		{
			GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CantDoThat, FindObjectOfType<Player>().Stats.Class);
			Rigidbody rb = GetComponent<Rigidbody>();
			if (!rb) rb = gameObject.AddComponent<Rigidbody>();
			rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
			Destroy(rb, 2);
		}
	}
}
