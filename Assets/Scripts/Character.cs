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
	public int Unallocated;
	[Header("Level/Experience")]
	public int Level;
	public int Experience;
	public int Gold;
	[Header("Calculated Stats")]
	public int BaseLife;
	public int BaseMana;
	public int ArmourClass;
	public int ToHitPercent;
	public int MinDamage;
	public int MaxDamage;
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
		SetStats(cc, "");
	}

	public Character(CharacterClass _class, string name)
	{
		SetStats(_class, name);
	}

	public Character(CharacterClass _class)
	{
		SetStats(_class, "");
	}

	void SetStats(CharacterClass _class, string name)
	{
		Name = "Player";
		Class = _class;
		Level = 1;
		Experience = 0;
		Recalculate();
		//Debug.Log("creating " + _class + " character");
		switch (_class)
		{
			case CharacterClass.NPC:
				Strength = 10;
				Dexterity = 10;
				Vitality = 10;
				GameDataTables.EnemyStats stats = GameDataTables.EnemyStats.Find(name);
				if (stats != null)
				{
					Name = name;
					Level = stats.Level;
					Life = BaseLife = stats.MaxHP;
					ArmourClass = stats.ArmourClass;
					ToHitPercent = stats.ToHitPercent;
					MinDamage = stats.MinDamage;
					MaxDamage = stats.MaxDamage;
					GivesExperience = stats.BaseExp;
				}
				else
				{
					Name = "NPC";
					Level = 1;
					Life = BaseLife = 10;
					GivesExperience = 1;
				}
				return;
			case CharacterClass.Warrior:
				Gold = 100;
				Strength = 30;
				Magic = 10;
				Dexterity = 20;
				Vitality = 25;
				Life = BaseLife = 70;
				BaseMana = 10;
				break;
			case CharacterClass.Rogue:
				Gold = 100;
				Strength = 20;
				Magic = 15;
				Dexterity = 30;
				Vitality = 20;
				Life = BaseLife = 100;
				BaseMana = 10;
				break;
			case CharacterClass.Sorceror:
				Gold = 100;
				Strength = 15;
				Magic = 35;
				Dexterity = 15;
				Vitality = 20;
				Life = BaseLife = 100;
				BaseMana = 10;
				break;
			default: break;
		}
	}

	public void ResetTimers()
	{
		AttackCooldown = Time.time;
	}

	public void Recalculate()
	{
		if (Life > BaseLife) Life = BaseLife;
		if (Class == CharacterClass.NPC)
		{
			// presets are probably fine for now. check later
		}
		else
		{
			ArmourClass = 4; // Dexterity; // needs proper calculation based on dex?
			ArmourClass += (Equipped.lefthand) ? Equipped.lefthand.Armour : 0;
			ArmourClass += (Equipped.righthand) ? Equipped.righthand.Armour : 0;
			ArmourClass += (Equipped.head) ? Equipped.head.Armour : 0;
			ArmourClass += (Equipped.body) ? Equipped.body.Armour : 0;

			ToHitPercent = 60; //Dexterity * 2; // needs proper calculation based on dex?

			MinDamage = 0;
			MinDamage += (Equipped.lefthand) ? Equipped.lefthand.MinDamage : 0;
			MinDamage += (Equipped.righthand) ? Equipped.righthand.MinDamage : 0;
			MinDamage += (Equipped.head) ? Equipped.head.MinDamage : 0;
			MinDamage += (Equipped.body) ? Equipped.body.MinDamage : 0;

			MaxDamage = 0; //Strength;// needs proper calculation based on str and dex?
			MaxDamage += (Equipped.lefthand) ? Equipped.lefthand.MaxDamage : 0;
			MaxDamage += (Equipped.righthand) ? Equipped.righthand.MaxDamage : 0;
			MaxDamage += (Equipped.head) ? Equipped.head.MaxDamage : 0;
			MaxDamage += (Equipped.body) ? Equipped.body.MaxDamage : 0;

			if (MinDamage < 1) MinDamage = 1;
			if (MaxDamage < 1) MaxDamage = 1;
			if (MinDamage > MaxDamage) MaxDamage = MinDamage;
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
			Unallocated += 5;
			Debug.Log("character leveled up - something interesting should happen");
		}
	}

	public int NextLevel()
	{
		if (Level >= GameDataTables.Levels.Length) return int.MaxValue;
		return GameDataTables.Levels[Level].Experience;
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
		// reinclude armour class later
		//float chance = (float)ToHitPercent / (float)defender.ArmourClass;
		float chance = (float)ToHitPercent;
		float roll = UnityEngine.Random.Range(0, 100f);
		Debug.Log("chance=" + chance + ",roll=" + roll);
		if (roll <= chance)
		{
			hit = true;
			// and then calculate the damage
			//damage = Damage - defender.ArmourClass;
			damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
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
