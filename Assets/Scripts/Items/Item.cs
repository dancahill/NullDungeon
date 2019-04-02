using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	public string Name = "New Item";
	public Sprite Icon = null;

	public virtual bool Use()
	{
		Debug.Log("trying to use " + name);
		return false;
	}
}
