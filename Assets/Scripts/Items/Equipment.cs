using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
	public enum EquipmentType
	{
		None,
		MeleeWeapon,
		RangedWeapon,
		Helmet,
		Shield
	}
	public EquipmentType equipmentType;
	public int Armour;
	public int MinDamage;
	public int MaxDamage;

	public override bool Use()
	{
		base.Use();
		Character c = GameManager.instance.PlayerCharacter;
		if (equipmentType == EquipmentType.MeleeWeapon)
		{
			if (c.Equipped.righthand != null)
			{
				c.Inventory.Add(c.Equipped.righthand);
			}
			c.Equipped.righthand = this;
			return true;
		}
		else if (equipmentType == EquipmentType.Shield)
		{
			if (c.Equipped.lefthand != null)
			{
				c.Inventory.Add(c.Equipped.lefthand);
			}
			c.Equipped.lefthand = this;
			return true;
		}
		//if (e.equipmentType == Equipment.EquipmentType.Shield && Equipped.righthand == null)
		//{
		//	Equipped.righthand = e;
		//	return true;
		//}
		return false;
	}
}
