using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image icon;
	public Button removeButton;
	Item item;

	public GameObject panel;
	public Text text;

	private void Awake()
	{
		panel.SetActive(false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		print("I was clicked");
	}

	public void OnDrag(PointerEventData eventData)
	{
		print("I'm being dragged!");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (item != null)
		{
			text.text = item.Name;
			if (item.GetType() == typeof(Equipment))
			{
				Equipment e = (Equipment)item;
				text.text += "\n" + e.equipmentType;
				text.text += "\nArmour: " + e.Armour;
				text.text += "\nDamage: " + e.MaxDamage;
			}
			panel.SetActive(true);
		}
		else
		{
			panel.SetActive(false);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		panel.SetActive(false);
	}

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
		if (item.Use())
		{
			GameManager.instance.PlayerCharacter.InventoryRemove(item);
			GameManager.instance.PlayerCharacter.Recalculate();
		}
	}
}
