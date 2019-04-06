using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemBase : ScriptableObject
{
	public string Name = "New Item";
	public Sprite Icon = null;
	[Header("Requirements")]
	public int RequiresLevel;
	public int RequiresStr;
	public int RequiresDex;
	public int RequiresMag;
	public int RequiresVit;
}
