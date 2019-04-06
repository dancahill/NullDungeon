using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Shield")]
public class ShieldBase : ItemBase
{
	[Header("Shield Stats")]
	public int Durability;
	public int Armour;
}
