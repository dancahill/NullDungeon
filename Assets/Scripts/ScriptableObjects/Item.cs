using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	public string Name = "New Item";
	public int Armour;
	public int MinDamage;
	public int MaxDamage;
	public Sprite Icon = null;
}
