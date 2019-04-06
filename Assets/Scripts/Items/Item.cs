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
		if (baseType.GetType() == typeof(WeaponBase))
		{
			if (c.Equipped.righthand != null && c.Equipped.righthand.baseType) c.Inventory.Add(c.Equipped.righthand);
			c.Equipped.righthand = this;
			return true;
		}
		else if (baseType.GetType() == typeof(ShieldBase))
		{
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
			return true;
		}
		else
		{
			Debug.Log(baseType.Name + " should be consumed but i don't know how");
		}
		return false;
	}
}
