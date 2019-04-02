using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	public Image icon;
	public Button removeButton;
	Item item;

	public void SetItem(Item item)
	{
		if (item != null)
		{
			this.item = item;
			icon.sprite = item.Icon;
			icon.enabled = true;
			removeButton.interactable = true;
		}
		else
		{
			ClearItem();
		}
	}

	public void ClearItem()
	{
		item = null;
		icon.sprite = null;
		icon.enabled = false;
		removeButton.interactable = false;
	}

	public void OnRemoveButton()
	{
		Character c = GameManager.instance.PlayerCharacter;
		Debug.Log("OnRemoveButton() '" + name + "'");
		if (name.StartsWith("InventorySlot"))
		{
			c.InventoryRemove(item);
		}
		else if (name == "LeftHandSlot")
		{
			c.InventoryAdd(item);
			c.Equipped.lefthand = null;
		}
		else if (name == "RightHandSlot")
		{
			c.InventoryAdd(item);
			c.Equipped.righthand = null;
		}
	}

	public void UseItem()
	{
		if (item == null) return;
		Debug.Log("don't know how to use " + name + " yet");
	}
}
