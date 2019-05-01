using UnityEngine;

public class EnemyModel : MonoBehaviour
{
	public GameObject weaponPosition;
	public GameObject weaponPrefab;
	public GameObject weaponEquipped;

	public void EquipSword(bool equip)
	{
		if (equip)
		{
			if (!weaponEquipped) weaponEquipped = Instantiate(weaponPrefab, weaponPosition.transform);
		}
		else
		{
			if (weaponEquipped) Destroy(weaponEquipped);
		}
	}
}
