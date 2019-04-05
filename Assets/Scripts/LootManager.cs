using UnityEngine;

public class LootManager : MonoBehaviour
{
	GameObject loot;
	[Header("Prefabs")]
	public GameObject healthPotion;
	public GameObject shield;
	public GameObject sword;

	void Awake()
	{
		loot = GameObject.Find("Loot");
	}

	public bool DropItem(Vector3 droplocation, Item item)
	{
		GameObject prefab;

		//Debug.Log("a=" + item.GetInstanceID() + ",b=" + healthPotion.GetComponent<Loot>().item.GetInstanceID());
		if (item == healthPotion.GetComponent<Loot>().item)
		{
			prefab = healthPotion;
		}
		else if (item == shield.GetComponent<Loot>().item)
		{
			prefab = shield;
		}
		else if (item == sword.GetComponent<Loot>().item)
		{
			prefab = sword;
		}
		else
		{
			Debug.Log("can't find a prefab for " + item.Name);
			return false;
		}
		GameObject g = Instantiate(prefab, new Vector3(droplocation.x + .5f, 0, droplocation.z + .5f), Quaternion.identity, loot.transform);
		g.name = item.Name;
		return true;
	}

	public void DropRandom(Vector3 droplocation, int itemlevel)
	{
		float roll = Random.Range(0, 100);
		if (roll < 5)
		{
			DropItem(droplocation, shield.GetComponent<Loot>().item);
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.DropRing);
		}
		else if (roll < 10)
		{
			DropItem(droplocation, sword.GetComponent<Loot>().item);
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.DropRing);
		}
		else if (roll < 20)
		{
			DropItem(droplocation, healthPotion.GetComponent<Loot>().item);
			SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.DropRing);
		}
	}
}
