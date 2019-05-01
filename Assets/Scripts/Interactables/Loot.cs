using System.Collections;
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
			rb.AddForce(Vector3.up * 4, ForceMode.Impulse);
			//Destroy(rb, 2);
			StartCoroutine(FinishDrop(gameObject));
			FindObjectOfType<LootManager>().FlipSound(item);
		}
	}

	IEnumerator FinishDrop(GameObject item)
	{
		//yield return new WaitForSeconds(1f);
		while (item.transform.position.y >= 0)
		{
			yield return null;
		}
		item.transform.Translate(Vector3.up * -item.transform.position.y);
		Rigidbody rb = item.GetComponent<Rigidbody>();
		if (rb) Destroy(rb);
	}
}
