using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class ConsumableBase : ItemBase
{
	public enum ConsumableType
	{
		None,
		Potion,
		Scroll,
	}
	public ConsumableType consumableType;
}
