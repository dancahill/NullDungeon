using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
	[Serializable]
	public struct EquippedItems
	{
		public Equipment lefthand;
		public Equipment righthand;
		public Equipment head;
		public Equipment body;
	}
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
	public EquippedItems Equipped;
	public List<Item> Inventory = new List<Item>();
	public List<Item> Stash = new List<Item>();
	readonly int InventoryMaxSize = 16;
	readonly int StashMaxSize = 32;
	[NonSerialized] public float AttackCooldown;

	public Character(string _class)
	{
		CharacterClass cc = (CharacterClass)Enum.Parse(typeof(CharacterClass), _class);
		SetStats(cc);
	}

	public Character(CharacterClass _class)
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
				Life = BaseLife = 50;
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

		ArmourClass += (Equipped.lefthand) ? Equipped.lefthand.Armour : 0;
		ArmourClass += (Equipped.righthand) ? Equipped.righthand.Armour : 0;
		ArmourClass += (Equipped.head) ? Equipped.head.Armour : 0;
		ArmourClass += (Equipped.body) ? Equipped.body.Armour : 0;

		Damage += (Equipped.lefthand) ? Equipped.lefthand.MaxDamage : 0;
		Damage += (Equipped.righthand) ? Equipped.righthand.MaxDamage : 0;
		Damage += (Equipped.head) ? Equipped.head.MaxDamage : 0;
		Damage += (Equipped.body) ? Equipped.body.MaxDamage : 0;
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
	/// <param name="cooldown">seconds to wait between attacks</param>
	/// <param name="damage">actual damage done to defender</param>
	/// <returns>true if hit</returns>
	public bool CalculateDamage(Character defender, float cooldown, out int damage)
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

	public bool InventoryAdd(Item item)
	{
		//Debug.Log("typeof='" + item.GetType() + "'");
		if (item.GetType() == typeof(Equipment))
		{
			Equipment e = (Equipment)item;
			if (e.equipmentType == Equipment.EquipmentType.MeleeWeapon && Equipped.righthand == null)
			{
				Equipped.righthand = e;
				return true;
			}
			if (e.equipmentType == Equipment.EquipmentType.Shield && Equipped.lefthand == null)
			{
				Equipped.lefthand = e;
				return true;
			}
		}
		if (Inventory.Count >= InventoryMaxSize) return false;
		Inventory.Add(item);
		return true;
	}

	public bool InventoryRemove(Item item)
	{
		Inventory.Remove(item);
		return false;
	}
}
