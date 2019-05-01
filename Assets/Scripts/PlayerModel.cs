using UnityEngine;

public class PlayerModel : MonoBehaviour
{
	public GameObject weaponPosition;
	public GameObject weaponRestPosition;
	public GameObject weaponPrefab;
	public GameObject weaponEquipped;

	public GameObject shieldPosition;
	public GameObject shieldRestPosition;
	public GameObject shieldPrefab;
	public GameObject shieldEquipped;

	public void EquipSword(bool equip)
	{
		bool intown = SceneController.GetActiveSceneName() == "Town";
		if (equip)
		{
			if (!weaponEquipped) weaponEquipped = Instantiate(weaponPrefab, (intown ? weaponRestPosition : weaponPosition).transform);
		}
		else
		{
			if (weaponEquipped) Destroy(weaponEquipped);
		}
	}

	public void EquipShield(bool equip)
	{
		bool intown = SceneController.GetActiveSceneName() == "Town";
		if (equip)
		{
			if (!shieldEquipped) shieldEquipped = Instantiate(shieldPrefab, (intown ? shieldRestPosition : shieldPosition).transform);
		}
		else
		{
			if (shieldEquipped) Destroy(shieldEquipped);
		}
	}
}
