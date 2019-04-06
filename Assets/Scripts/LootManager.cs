﻿using UnityEngine;

public class LootManager : MonoBehaviour
{
	LootCatalog catalog;

	private void Awake()
	{
		catalog = FindObjectOfType<LootCatalog>();
	}

	public bool DropItem(Vector3 droplocation, Item item)
	{
		GameObject prefab = catalog.FindPrefab(item);
		if (prefab == null) return false;
		GameObject loot = GameObject.Find("Loot");
		GameObject g = Instantiate(prefab, new Vector3(droplocation.x + .5f, 0, droplocation.z + .5f), Quaternion.identity, loot.transform);
		Loot l = g.GetComponent<Loot>();
		g.name = item.baseTypeName.ToUpper();
		l.text.text = item.baseTypeName.ToUpper();
		l.item = item;
		if (item.baseType.GetType() == typeof(ShieldBase))
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.FlipShield);
		else if (item.baseType.GetType() == typeof(WeaponBase))
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.FlipSword);
		else if (item.baseType.GetType() == typeof(ConsumableBase))
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.FlipPotion);
		else
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.FlipRing);
		return true;
	}

	public void DropRandom(Vector3 droplocation, int itemlevel)
	{
		// need much better randomization of drops than this
		// should probably also set random stats like durability before dropping
		float roll = Random.Range(0, 100);
		if (roll < 5)
		{
			Item item = catalog.FindItem("Potion Of Healing");
			DropItem(droplocation, item);
		}
		else if (roll < 10)
		{
			Item item = catalog.FindItem("Buckler");
			item.durability = Random.Range(((ShieldBase)item.baseType).Durability / 2, ((ShieldBase)item.baseType).Durability);
			DropItem(droplocation, item);
		}
		else if (roll < 15)
		{
			Item item = catalog.FindItem("Short Sword");
			item.durability = Random.Range(((WeaponBase)item.baseType).Durability / 2, ((WeaponBase)item.baseType).Durability);
			DropItem(droplocation, item);
		}
		else if (roll < 20)
		{
			Item item = catalog.FindItem("Bastard Sword");
			item.durability = Random.Range(((WeaponBase)item.baseType).Durability / 2, ((WeaponBase)item.baseType).Durability);
			DropItem(droplocation, item);
		}
	}
}
