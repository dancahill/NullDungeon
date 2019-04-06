using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
	[Serializable]
	public struct EquippedItems
	{
		public Item head;
		public Item neck;
		public Item body;
		public Item righthand;
		public Item lefthand;
		public Item rightfinger;
		public Item leftfinger;
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
	public float AttackSpeed;
	[Header("Life")]
	public float Life;
	public float Mana;
	[Header("NPC Values")]
	public int GivesExperience;
	[Header("Inventory")]
	public EquippedItems Equipped;
	public List<Item> Inventory = new List<Item>();
	public List<Item> Stash = new List<Item>();
	readonly int InventoryMaxSize = 40;
	readonly int BeltMaxSize = 8;
	readonly int StashMaxSize = 32;
	[NonSerialized] public float WalkCooldown;
	[NonSerialized] public float AttackCooldown;
	[NonSerialized] public float HitRecoveryCooldown;
	[NonSerialized] public float BlockCooldown;

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
		if (_class == CharacterClass.NPC)
		{
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
				AttackSpeed = stats.AttackTime;
			}
			else
			{
				Name = "NPC";
				Level = 1;
				Life = BaseLife = 10;
				GivesExperience = 1;
			}
			return;
		}
		switch (_class)
		{
			case CharacterClass.Warrior:
				Gold = 100;
				Strength = 30;
				Magic = 10;
				Dexterity = 20;
				Vitality = 25;
				break;
			case CharacterClass.Rogue:
				Gold = 100;
				Strength = 20;
				Magic = 15;
				Dexterity = 30;
				Vitality = 20;
				break;
			case CharacterClass.Sorceror:
				Gold = 100;
				Strength = 15;
				Magic = 35;
				Dexterity = 15;
				Vitality = 20;
				break;
			default: break;
		}
		Recalculate();
		Life = BaseLife;
		Mana = BaseMana;

		// gonna need a cleaner method for this later
		LootCatalog catalog = GameObject.FindObjectOfType<LootCatalog>();
		if (catalog)
		{
			Equipped.righthand = catalog.FindItem("Short Sword");
			Equipped.righthand.durability = ((WeaponBase)Equipped.righthand.baseType).Durability;
			Equipped.lefthand = catalog.FindItem("Buckler");
			Equipped.lefthand.durability = ((ShieldBase)Equipped.lefthand.baseType).Durability;
		}
	}

	public void ResetTimers()
	{
		WalkCooldown = Time.time;
		AttackCooldown = Time.time;
		HitRecoveryCooldown = Time.time;
		BlockCooldown = Time.time;
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
			int vititems = 0;
			int lifeitems = 0;
			int magitems = 0;
			int manaitems = 0;
			int acitems = 0;
			int tohititems = 0;
			int chardamage = 0;
			int weaponmin = 0;
			int weaponmax = 0;

			acitems += (Equipped.lefthand != null && Equipped.lefthand.baseType) ? ((ShieldBase)Equipped.lefthand.baseType).Armour : 0;
			//acitems += (Equipped.righthand) ? Equipped.righthand.Armour : 0;
			//acitems += (Equipped.head) ? Equipped.head.Armour : 0;
			//acitems += (Equipped.body) ? Equipped.body.Armour : 0;

			switch (Class)
			{
				case CharacterClass.Warrior:
					BaseLife = 2 * Vitality + 2 * vititems + 2 * Level + lifeitems + 18;
					BaseMana = 1 * Magic + 1 * magitems + 1 * Level + manaitems - 1;
					chardamage = Strength * Level / 100; // melee - should be different for bow
					AttackSpeed = 0.45f; // should also include weapon speed
					break;
				case CharacterClass.Rogue:
					BaseLife = 1 * Vitality + (int)(1.5f * (float)vititems) + 2 * Level + lifeitems + 23;
					BaseMana = 1 * Magic + (int)(1.5f * (float)magitems) + 2 * Level + manaitems + 5;
					chardamage = (Strength + Dexterity) * Level / 200; // melee - should be different for bow
					AttackSpeed = 0.5f; // should also include weapon speed
					break;
				case CharacterClass.Sorceror:
					BaseLife = 1 * Vitality + 1 * vititems + 1 * Level + lifeitems + 9;
					BaseMana = 2 * Magic + 2 * magitems + 2 * Level + manaitems - 2;
					chardamage = Strength * Level / 100; // melee - should be different for bow
					AttackSpeed = 0.6f; // should also include weapon speed
					break;
				default: break;
			}
			ArmourClass = Dexterity / 5 + acitems;
			// to hit is actually more complicated, but this is what goes on the screen
			ToHitPercent = 50 + Dexterity / 2 * tohititems;

			//weaponmin += (Equipped.lefthand) ? Equipped.lefthand.MinDamage : 0;
			weaponmin += (Equipped.righthand != null && Equipped.righthand.baseType) ? ((WeaponBase)Equipped.righthand.baseType).MinDamage : 0;
			//weaponmin += (Equipped.head) ? Equipped.head.MinDamage : 0;
			//weaponmin += (Equipped.body) ? Equipped.body.MinDamage : 0;

			//weaponmax += (Equipped.lefthand) ? Equipped.lefthand.MaxDamage : 0;
			weaponmax += (Equipped.righthand != null && Equipped.righthand.baseType) ? ((WeaponBase)Equipped.righthand.baseType).MaxDamage : 0;
			//weaponmax += (Equipped.head) ? Equipped.head.MaxDamage : 0;
			//weaponmax += (Equipped.body) ? Equipped.body.MaxDamage : 0;

			if (weaponmin < 1) weaponmin = 1; // unarmed
			if (weaponmax < weaponmin) weaponmax = weaponmin;

			MinDamage = chardamage + weaponmin;
			MaxDamage = chardamage + weaponmax;
		}
	}

	public bool CanStep(float speed)
	{
		if (Time.time < WalkCooldown) return false;
		//WalkCooldown = Time.time + Mathf.Clamp(1f / speed, 0, 0.4f);
		// should be 0.4f, but since max speed is 2, 0.5f works better for now
		WalkCooldown = Time.time + Mathf.Clamp(1f / speed, 0, 0.5f);
		return true;
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

	public bool TryBlocking(Character defender)
	{
		float chance = defender.Dexterity + 2 * (Level - defender.Level) + 30;// 30 for warriors, 20 for rogues, 10 for sorcerors
		float roll = UnityEngine.Random.Range(0, 100f);
		Debug.Log("chance to block =" + chance + ",roll=" + roll);
		if (defender.Equipped.lefthand != null && defender.Equipped.lefthand.baseType && defender.Equipped.lefthand.baseType.GetType() == typeof(ShieldBase))
		{
			defender.Equipped.lefthand.durability -= 0.1f;
			if (defender.Equipped.lefthand.durability <= 0)
			{
				defender.Equipped.lefthand.durability = 0;
				defender.InventoryAdd(defender.Equipped.lefthand);
				defender.Equipped.lefthand = null;
			}
		}
		if (roll <= chance)
		{
			Debug.Log("good for a block");
			return true;
		}
		return false;
	}

	/// <summary>
	/// full comparison and calculation of attacker and defender stats
	/// </summary>
	/// <param name="defender">defender's character stats</param>
	/// <param name="cooldown">seconds to wait between attacks</param>
	/// <param name="damage">actual damage done to defender</param>
	/// <returns>true if hit</returns>
	public bool CalculateDamage(Character defender, out int damage)
	{
		Recalculate();
		bool hit;

		if (Time.time < AttackCooldown)
		{
			hit = false;
			damage = 0;
			return hit;
		}
		AttackCooldown = Time.time + AttackSpeed;

		// npcs can attack as fast as AttackSpeed, but it looks and feels wrong
		if (Class == CharacterClass.NPC) AttackCooldown += UnityEngine.Random.Range(0f, 0.5f);
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
		//Debug.Log("chance to hit =" + chance + ",roll=" + roll);
		if (roll <= chance)
		{
			if (defender.Class != CharacterClass.NPC && TryBlocking(defender))
			{
				damage = 0;
				Debug.Log("good for a hit, but it's blocked");
				hit = false;
			}
			else
			{
				damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
				if (Class == CharacterClass.Warrior)
				{
					chance = (float)Level;
					roll = UnityEngine.Random.Range(0, 100f);
					if (roll <= chance)
					{
						Debug.Log("critical hit - double damage");
						damage *= 2;
					}
				}
				Debug.Log("good for a hit. damage is " + damage);
				hit = true;
			}
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
		if (item != null && item.baseType != null)
		{
			if (item.baseType.GetType() == typeof(WeaponBase) && Equipped.righthand == null)
			{
				Equipped.righthand = item;
				return true;
			}
			else if (item.baseType.GetType() == typeof(ShieldBase) && Equipped.lefthand == null)
			{
				Equipped.lefthand = item;
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
