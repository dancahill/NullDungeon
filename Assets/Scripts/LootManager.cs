using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
	public List<GameObject> prefabs;
	public List<Item> items;

	public bool DropItem(Vector3 droplocation, Item item)
	{
		if (item == null) return false;
		GameObject prefab = prefabs.Find(p => p.GetComponent<Loot>().item.baseType.GetType() == item.baseType.GetType());
		if (prefab == null)
		{
			Debug.Log("can't find a prefab for " + ((item.baseType) ? item.baseType.Name : "Unknown"));
			return false;
		}
		GameObject loot = GameObject.Find("Loot");
		GameObject g = Instantiate(prefab, new Vector3(droplocation.x + .5f, 0, droplocation.z + .5f), Quaternion.identity, loot.transform);
		Loot l = g.GetComponent<Loot>();
		g.name = item.baseType.Name.ToUpper();
		l.text.text = item.baseType.Name.ToUpper();
		l.item = item;
		SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.DropRing);
		return true;
	}

	public void DropRandom(Vector3 droplocation, int itemlevel)
	{
		Item finditem(string s)
		{
			Item x = items.Find(i => i.baseType.name == s);
			if (x == null) Debug.Log("can't find item '" + s + "'");
			return x;
		}
		// need much better randomization of drops than this
		// should probably also set random stats like durability before dropping
		float roll = Random.Range(0, 100);
		if (roll < 5)
		{
			DropItem(droplocation, finditem("Potion Of Healing"));
		}
		else if (roll < 10)
		{
			DropItem(droplocation, finditem("Buckler"));
		}
		else if (roll < 15)
		{
			DropItem(droplocation, finditem("Short Sword"));
		}
		else if (roll < 20)
		{
			DropItem(droplocation, finditem("Bastard Sword"));
		}
	}
}
