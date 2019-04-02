using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	public enum EquipmentType
	{
		None,
		MeleeWeapon,
		RangedWeapon,
		Helmet,
		Shield
	}
	public string Name = "New Item";
	public EquipmentType equipmentType;
	public Sprite Icon = null;
	public int Armour;
	public int MinDamage;
	public int MaxDamage;
}
