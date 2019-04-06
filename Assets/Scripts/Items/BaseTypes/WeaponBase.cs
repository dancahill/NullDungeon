using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Weapon")]
public class WeaponBase : ItemBase
{
	[Header("Weapon Stats")]
	public int Durability;
	public int MinDamage;
	public int MaxDamage;
}
