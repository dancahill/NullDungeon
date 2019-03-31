using System;
using UnityEngine;

[Serializable]
public class DungeonObject
{
	public string Name;
	public CharacterStats.CharacterClass Class;
	public int BaseLife;
	public float Life;
	public double x;
	public double y;
	public double z;
}

[Serializable]
public class DungeonLevel
{
	public DungeonObject[] DungeonObjects;
}

[Serializable]
public class DungeonState
{
	public DungeonLevel[] DungeonLevels;

	public DungeonState()
	{
		DungeonLevels = new DungeonLevel[17];
	}

	public void LoadState(string scenename)
	{
		GameObject enemies = GameObject.Find("Enemies");
		if (!enemies) return;
		int level = 0;
		string result = System.Text.RegularExpressions.Regex.Match(scenename, @"\d+$").Value;
		if (result != "") level = int.Parse(result);
		DungeonLevel dl = DungeonLevels[level];
		if (dl == null || dl.DungeonObjects == null || dl.DungeonObjects.Length < 1)
		{
			//Debug.Log("empty level in save. not replacing enemies");
			return;
		}
		for (int i = 0; i < enemies.transform.childCount; i++)
		{
			Transform enemy = enemies.transform.GetChild(i);
			//Debug.Log("destroying child object enemy=" + enemy.name);
			Enemy e = enemy.GetComponentInChildren<Enemy>();
			UnityEngine.Object.Destroy(enemy.gameObject);
		}
		EnemyPrefabs ep = GameObject.FindObjectOfType<EnemyPrefabs>();
		foreach (DungeonObject d in dl.DungeonObjects)
		{
			GameObject prefab;
			if (d.Name == "Skeleton")
			{
				prefab = ep.m_SkeletonPrefab;
			}
			else if (d.Name == "Zombie")
			{
				prefab = ep.m_ZombiePrefab;
			}
			else
			{
				Debug.Log("prefab for " + d.Name + " not found");
				continue;
			}
			GameObject g = GameObject.Instantiate(prefab, new Vector3((float)d.x, (float)d.y, (float)d.z), Quaternion.identity, enemies.transform);
			g.name = d.Name;
			Enemy e = g.GetComponentInChildren<Enemy>();
			e.Stats.Life = d.Life;
		}
	}

	public void SaveState(string scenename)
	{
		GameObject enemies = GameObject.Find("Enemies");
		if (!enemies) return;
		DungeonLevel dl = new DungeonLevel
		{
			DungeonObjects = new DungeonObject[enemies.transform.childCount]
		};
		//Debug.Log("enemies.transform.childCount=" + enemies.transform.childCount + ", x=" + x);
		//foreach (Transform enemy in enemies.transform)
		for (int i = 0; i < enemies.transform.childCount; i++)
		{
			Transform enemy = enemies.transform.GetChild(i);
			//Debug.Log("child object enemy=" + enemy.name);
			Enemy e = enemy.GetComponentInChildren<Enemy>();
			DungeonObject d = new DungeonObject
			{
				Name = enemy.name,
				Class = e.Stats.Class,
				BaseLife = e.Stats.BaseLife,
				Life = e.Stats.Life,
				x = Math.Round(enemy.position.x, 2),
				y = Math.Round(enemy.position.y, 2),
				z = Math.Round(enemy.position.z, 2),
			};
			dl.DungeonObjects[i] = d;
		}
		int level = 0;
		string result = System.Text.RegularExpressions.Regex.Match(scenename, @"\d+$").Value;
		if (result != "") level = int.Parse(result);
		DungeonLevels[level] = dl;
		GameSave.SaveDungeon();
		return;
	}
}
