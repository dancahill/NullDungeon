using UnityEngine;

public class PlayerModel : MonoBehaviour
{
	public GameObject weaponPosition;
	public GameObject weaponPrefab;
	public GameObject weaponEquipped;

	public GameObject ShieldPosition;
	public GameObject ShieldPrefab;
	public GameObject ShieldEquipped;

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

	public void EquipShield(bool equip)
	{
		if (equip)
		{
			if (!ShieldEquipped) ShieldEquipped = Instantiate(ShieldPrefab, ShieldPosition.transform);
		}
		else
		{
			if (ShieldEquipped) Destroy(ShieldEquipped);
		}
	}
}
