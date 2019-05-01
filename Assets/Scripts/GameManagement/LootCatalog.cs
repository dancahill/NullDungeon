using System.Collections.Generic;
using UnityEngine;

public class LootCatalog : MonoBehaviour
{
	public List<GameObject> prefabs;
	public List<Item> items;

	public GameObject FindPrefab(Item item)
	{
		if (item == null) return null;
		GameObject prefab = prefabs.Find(p => p.GetComponent<Loot>().item.baseType.GetType() == item.baseType.GetType());
		if (prefab == null) Debug.Log("can't find a prefab for " + ((item.baseType) ? item.baseType.Name : "Unknown"));
		return prefab;
	}

	public Item FindItem(string itemname)
	{
		Item item = items.Find(i => i.baseType.name == itemname);
		if (item == null) Debug.Log("can't find item '" + itemname + "'");
		else item.baseTypeName = item.baseType.Name;
		return item;
	}
}
