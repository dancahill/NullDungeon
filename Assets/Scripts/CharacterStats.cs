using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStats
{
	public enum CharacterClass
	{
		NPC,
		Warrior,
		Rogue,
		Sorceror,
	}
	[Header("Character Class")]
	public string Name;
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
	public int ArmourClass;
	public int ToHitPercent;
	public int Damage;
	[Header("Life")]
	public float Life;
	public float Mana;
	[Header("NPC Values")]
	public int GivesExperience;
	[Header("Inventory")]
	public List<Item> Equipped = new List<Item>();
	public List<Item> Inventory = new List<Item>();
	//public CharacterItem[] Equipped;
	//public CharacterItem[] Inventory;
	//[Header("...")]
	[NonSerialized] public float AttackCooldown;

	public CharacterStats(string _class)
	{
		CharacterClass cc = (CharacterClass)Enum.Parse(typeof(CharacterClass), _class);
		SetStats(cc);
	}

	public CharacterStats(CharacterClass _class)
	{
		SetStats(_class);
	}

	void SetStats(CharacterClass _class)
	{
		Name = "Player";
		Class = _class;
		Experience = 0;
		Recalculate();
		//Debug.Log("creating " + _class + " character");
		switch (_class)
		{
			case CharacterClass.NPC:
				Name = "NPC";
				GivesExperience = 100;
				Level = 1;
				Strength = 10 + (5 * Level);
				Dexterity = 20 + (5 * Level);
				Vitality = 10 + (5 * Level);
				Life = BaseLife = 100;
				return;
			case CharacterClass.Warrior:
				Strength = 30;
				Magic = 10;
				Dexterity = 20;
				Vitality = 25;
				Life = BaseLife = 100;
				BaseMana = 10;
				break;
			case CharacterClass.Rogue:
				Strength = 20;
				Magic = 15;
				Dexterity = 30;
				Vitality = 20;
				Life = BaseLife = 100;
				BaseMana = 10;
				break;
			case CharacterClass.Sorceror:
				Strength = 15;
				Magic = 35;
				Dexterity = 15;
				Vitality = 20;
				Life = BaseLife = 100;
				BaseMana = 10;
				break;
			default: break;
		}
		//Equipped = new CharacterItem[4];
		//Inventory = new CharacterItem[8];
		//Equipped.Add(new Item { Name = "Rubber Chicken", MaxDamage = 5 });
		//Equipped.Add(new Item { Name = "Diaper", Armour = 5 });
		//Inventory.Add(new Item { Name = "Booger Collection" });
	}

	public void ResetTimers()
	{
		AttackCooldown = Time.time;
	}

	public void Recalculate()
	{
		// none of this really makes sense, but it's a start
		ArmourClass = Dexterity;
		ToHitPercent = Dexterity * 2;
		Damage = Strength;
		if (Life > BaseLife) Life = BaseLife;
		if (Equipped != null && Equipped.Count > 0)
		{
			foreach (Item ci in Equipped)
			{
				ArmourClass += ci.Armour;
				Damage += ci.MaxDamage;
			}
		}
	}

	public bool CanAttack()
	{
		if (Time.time < AttackCooldown) return false;
		if (Life == 0) return false;
		return true;
	}

	public void AddExperience(int experience)
	{
		Experience += experience;
		if (Experience >= NextLevel())
		{
			Level++;
			Debug.Log("character leveled up - something interesting should happen");
		}
	}

	public int NextLevel()
	{
		if (Level >= CharacterTables.Levels.Length) return int.MaxValue;
		return CharacterTables.Levels[Level].Experience;
	}

	/// <summary>
	/// full comparison and calculation of attacker and defender stats
	/// </summary>
	/// <param name="defender">defender's character stats</param>
	/// <param name="cooldown">seconds between attacks</param>
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
		float chance = (float)ToHitPercent / (float)defender.ArmourClass;
		float roll = UnityEngine.Random.Range(0, chance);
		Debug.Log("chance=" + chance + ",roll=" + roll);
		if (roll > 1f)
		{
			hit = true;
			// and then calculate the damage
			//damage = Damage - defender.ArmourClass;
			damage = Damage;
			Debug.Log("good for a hit. damage is " + damage);
			if (damage < 0) damage = 0;
		}
		else
		{
			hit = false;
			damage = 0;
		}
		return hit;
	}
}
