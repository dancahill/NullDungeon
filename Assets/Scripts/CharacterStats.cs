using UnityEngine;

[System.Serializable]
public class CharacterStats
{
	public enum CharacterClass
	{
		NPC,
		Warrior,
		Peon,
	}
	[Header("Character Class")]
	public CharacterClass Class;
	[Header("Base Stats")]
	public int Strength;
	public int Dexterity;
	public int Vitality;
	[Header("Level/Experience")]
	public int Level;
	public int Experience;

	[Header("Calculated Stats")]
	public int MaxLife;
	public int ArmorClass;
	public int ToHitPercent;
	public int Damage;
	[Header("Life")]
	public int Life;
	[Header("NPC Values")]
	public int GivesExperience;

	public CharacterStats(string _class)
	{
		CharacterClass cc = (CharacterClass)System.Enum.Parse(typeof(CharacterClass), _class);
		SetStats(cc);
	}

	public CharacterStats(CharacterClass _class)
	{
		SetStats(_class);
	}

	void SetStats(CharacterClass _class)
	{
		Class = _class;
		Strength = 10;
		Dexterity = 20;
		Vitality = 10;
		Level = 1;
		Experience = 0;
		MaxLife = 100;
		Life = MaxLife;
		Recalculate();
		Debug.Log("creating " + _class + " character");
		switch (_class)
		{
			case CharacterClass.NPC:
				GivesExperience = 100;
				break;
			case CharacterClass.Warrior:
				Strength = 25;
				Vitality = 20;
				break;
			case CharacterClass.Peon:
				break;
			default: break;
		}
	}

	void Recalculate()
	{
		//Life = Vitality * 2;
		ArmorClass = Dexterity;
		ToHitPercent = Dexterity * 2;
		Damage = Strength;
	}

	/// <summary>
	/// full comparison and calculation of attacker and defender stats
	/// </summary>
	/// <param name="defender">defender's character stats</param>
	/// <param name="damage">actual damage done to defender</param>
	/// <returns>true if hit</returns>
	public bool CalculateDamage(CharacterStats defender, out int damage)
	{
		Recalculate();
		bool hit;
		// calculate whether there was a hit;
		// just make some chance to miss for testing
		int r = Random.Range(1, 100);
		if (r > ToHitPercent)
		{
			hit = false;
			damage = 0;
		}
		else
		{
			hit = true;
			// and then calculate the damage
			damage = Damage;
		}
		return hit;
	}
}
