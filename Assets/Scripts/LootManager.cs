using UnityEngine;

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
		g.name = item.baseType.Name.ToUpper();
		l.text.text = item.baseType.Name.ToUpper();
		l.item = item;
		SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.DropRing);
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
			DropItem(droplocation, item);
		}
		else if (roll < 15)
		{
			Item item = catalog.FindItem("Short Sword");
			DropItem(droplocation, item);
		}
		else if (roll < 20)
		{
			Item item = catalog.FindItem("Bastard Sword");
			DropItem(droplocation, item);
		}
	}
}
