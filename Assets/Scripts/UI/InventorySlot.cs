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
				if (e.Armour > 0) text.text += "\nArmour: " + e.Armour;
				if (e.MaxDamage > 0) text.text += "\nDamage: " + e.MinDamage + "-" + e.MaxDamage;
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
		Character c = GameManager.GetPlayer();
		Debug.Log("OnRemoveButton() '" + name + "'");
		if (name.StartsWith("InventorySlot"))
		{
			LootManager lm = FindObjectOfType<LootManager>();
			if (lm.DropItem(GameObject.Find("Player").transform.position, item))
			{
				c.InventoryRemove(item);
				SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.DropRing);
			}
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
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Click);
			GameManager.instance.PlayerCharacter.InventoryRemove(item);
			GameManager.instance.PlayerCharacter.Recalculate();
		}
	}

	public void ConsumeItem()
	{
		if (item == null) return;
		if (item.Consume())
		{
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Click);
			GameManager.GetPlayer().InventoryRemove(item);
			GameManager.GetPlayer().Recalculate();
		}
	}
}
