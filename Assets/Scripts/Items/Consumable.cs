using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
	public enum ConsumableType
	{
		None,
		Potion,
		Scroll,
	}
	public ConsumableType consumableType;

	public override bool Use()
	{
		base.Use();
		return false;
	}

	public override bool Consume()
	{
		//return base.Consume();
		Character player = GameManager.GetPlayer();
		if (name == "Potion Of Healing")
		{
			float bonus = 2f;// bonus depends on character class; 2 for warrior, 1.5 for rogue, 1 for sorceror, but who cares?
			player.Life += (int)UnityEngine.Random.Range(bonus * (float)player.BaseLife / 8f, bonus * 3f * (float)player.BaseLife / 8f);
			return true;
		}
		else
		{
			Debug.Log(name + " should be consumed but i don't know how");
		}
		return false;
	}
}
