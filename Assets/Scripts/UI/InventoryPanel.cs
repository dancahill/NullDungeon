using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
	public Transform headSlot;
	public Transform leftHandSlot;
	public Transform rightHandSlot;
	public Transform bodySlot;
	public Transform itemsParent;
	Character player;
	InventorySlot[] slots;

	void Start()
	{
		player = GameManager.instance.PlayerCharacter;
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	void Update()
	{
		headSlot.GetComponent<InventorySlot>().SetItem(player.Equipped.head);
		leftHandSlot.GetComponent<InventorySlot>().SetItem(player.Equipped.lefthand);
		rightHandSlot.GetComponent<InventorySlot>().SetItem(player.Equipped.righthand);
		bodySlot.GetComponent<InventorySlot>().SetItem(player.Equipped.body);
		for (int i = 0; i < slots.Length; i++)
		{
			if (i < player.Inventory.Count)
			{
				slots[i].SetItem(player.Inventory[i]);
			}
			else
			{
				slots[i].ClearItem();
			}
		}
	}
}
