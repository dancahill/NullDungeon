using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Weapon")]
public class WeaponBase : ItemBase
{
	public int MinDamage;
	public int MaxDamage;
}
