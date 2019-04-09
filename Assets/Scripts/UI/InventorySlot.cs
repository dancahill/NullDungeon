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
			text.text = (item.baseType) ? item.baseType.Name : "Unknown";
			if (item.baseType.GetType() == typeof(WeaponBase))
			{
				WeaponBase e = (WeaponBase)item.baseType;
				string damage = "\nDamage: " + e.MinDamage + "-" + e.MaxDamage;
				damage += "  Dur: " + Mathf.CeilToInt(item.durability) + "/" + e.Durability;
				text.text += damage;
			}
			else if (item.baseType.GetType() == typeof(ShieldBase))
			{
				ShieldBase e = (ShieldBase)item.baseType;
				string armour = "\nArmour: " + e.Armour;
				armour += "  Dur: " + Mathf.CeilToInt(item.durability) + "/" + e.Durability;
				text.text += armour;
			}
			string required = "";
			if (item.baseType.RequiresStr > 0) required += (required.Length > 0 ? " " : "") + item.baseType.RequiresStr + " Str";
			if (item.baseType.RequiresDex > 0) required += (required.Length > 0 ? " " : "") + item.baseType.RequiresDex + " Dex";
			if (required.Length > 0) text.text += "\nRequired: " + required;
			text.text = text.text.ToUpper();
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
		if (item != null && item.baseType != null)
		{
			this.item = item;
			icon.sprite = item.baseType.Icon;
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
				//SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.FlipRing);
			}
		}
		else if (name == "HeadSlot")
		{
			c.InventoryAdd(item);
			c.Equipped.head = null;
		}
		else if (name == "BodySlot")
		{
			c.InventoryAdd(item);
			c.Equipped.body = null;
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

	public void EquipItem()
	{
		if (item != null && item.Equip())
		{
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Click);
			GameManager.GetPlayer().InventoryRemove(item);
			GameManager.GetPlayer().Recalculate();
		}
	}

	public void ConsumeItem()
	{
		if (item != null && item.Consume())
		{
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Click);
			GameManager.GetPlayer().InventoryRemove(item);
			GameManager.GetPlayer().Recalculate();
		}
	}
}
