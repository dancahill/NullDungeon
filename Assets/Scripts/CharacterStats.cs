using UnityEngine;

[System.Serializable]
public class CharacterStats
{
	public enum CharacterClass
	{
		NPC,
		Warrior,
		Rogue,
		Sorceror,
		Peon,
	}
	[Header("Character Class")]
	public CharacterClass Class;
	[Header("Base Stats")]
	public int Strength;
	public int Magic;
	public int Dexterity;
	public int Vitality;
	[Header("Level/Experience")]
	public int Level;
	public int Experience;

	[Header("Calculated Stats")]
	public int BaseLife;
	public int BaseMana;
	public int ArmorClass;
	public int ToHitPercent;
	public int Damage;
	[Header("Life")]
	public int Life;
	[Header("NPC Values")]
	public int GivesExperience;
	[Header("...")]
	public float AttackCooldown;

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
		BaseLife = 100;
		BaseMana = 1;
		Life = BaseLife;
		Recalculate();
		//Debug.Log("creating " + _class + " character");
		switch (_class)
		{
			case CharacterClass.NPC:
				GivesExperience = 100;
				break;
			case CharacterClass.Warrior:
				Strength = 30;
				Magic = 10;
				Dexterity = 20;
				Vitality = 25;
				break;
			case CharacterClass.Rogue:
				Strength = 20;
				Magic = 15;
				Dexterity = 30;
				Vitality = 20;
				break;
			case CharacterClass.Sorceror:
				Strength = 15;
				Magic = 35;
				Dexterity = 15;
				Vitality = 20;
				break;
			case CharacterClass.Peon:
				break;
			default: break;
		}
	}

	public void ResetTimers()
	{
		AttackCooldown = Time.time;
	}

	void Recalculate()
	{
		//Life = Vitality * 2;
		ArmorClass = Dexterity;
		ToHitPercent = Dexterity * 2;
		Damage = Strength;
	}

	public bool CanAttack()
	{
		if (Time.time < AttackCooldown) return false;
		return true;
	}

	public int NextLevel()
	{
		return 2000;
	}

	/// <summary>
	/// full comparison and calculation of attacker and defender stats
	/// </summary>
	/// <param name="defender">defender's character stats</param>
	/// <param name="damage">actual damage done to defender</param>
	/// <returns>true if hit</returns>
	public bool CalculateDamage(CharacterStats defender, float cooldown, out int damage)
	{
		Recalculate();
		bool hit;

		if (Time.time < AttackCooldown)
		{
			hit = false;
			damage = 0;
			return hit;
		}
		AttackCooldown = Time.time + cooldown;
		if (defender == null)
		{
			hit = false;
			damage = 0;
			return hit;
		}
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
