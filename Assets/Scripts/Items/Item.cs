using System;
using UnityEngine;

[Serializable]
public class Item
{
	// the instanceID of baseType that ends up in the serialized version is unreliable,
	// so use baseTypeName to relocate the proper baseType on reload
	public ItemBase baseType;
	public string baseTypeName;
	public float durability;

	public Item(ItemBase type)
	{
		baseType = type;
		baseTypeName = baseType.Name;
		if (baseType.GetType() == typeof(WeaponBase))
		{
			durability = 42;
		}
		else if (baseType.GetType() == typeof(ShieldBase))
		{
			durability = 41;
		}
	}

	public bool Equip()
	{
		Character c = GameManager.GetPlayer();

		bool met = true;
		if (baseType.RequiresStr > c.Strength) met = false;
		if (baseType.RequiresDex > c.Dexterity) met = false;
		if (baseType.RequiresMag > c.Magic) met = false;
		if (baseType.RequiresVit > c.Vitality) met = false;
		if (baseType.RequiresLevel > c.Level) met = false;
		if (!met)
		{
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.CantUseThisYet, c.Class);
			return false;
		}
		if (baseType.GetType() == typeof(WeaponBase))
		{
			if (durability <= 0)
			{
				SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.CantUseThisYet, c.Class);
				return false;
			}
			if (c.Equipped.righthand != null && c.Equipped.righthand.baseType) c.Inventory.Add(c.Equipped.righthand);
			c.Equipped.righthand = this;
			return true;
		}
		else if (baseType.GetType() == typeof(ShieldBase))
		{
			if (durability <= 0)
			{
				SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.CantUseThisYet, c.Class);
				return false;
			}
			if (c.Equipped.lefthand != null && c.Equipped.lefthand.baseType) c.Inventory.Add(c.Equipped.lefthand);
			c.Equipped.lefthand = this;
			return true;
		}
		return false;
	}

	public bool Consume()
	{
		Character player = GameManager.GetPlayer();
		if (baseType.Name == "Potion Of Healing")
		{
			float bonus = 2f;// bonus depends on character class; 2 for warrior, 1.5 for rogue, 1 for sorceror, but who cares?
			player.Life += (int)UnityEngine.Random.Range(bonus * (float)player.BaseLife / 8f, bonus * 3f * (float)player.BaseLife / 8f);
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.InvPotion);
			return true;
		}
		else
		{
			Debug.Log(baseType.Name + " should be consumed but i don't know how");
		}
		return false;
	}
}
