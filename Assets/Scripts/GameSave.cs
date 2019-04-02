using System.IO;
using UnityEngine;

public class GameSave
{
	//https://docs.unity3d.com/Manual/JSONSerialization.html
	public static void LoadCharacter()
	{
		string filepath = Application.persistentDataPath + "/character1.json";
		Debug.Log("reading " + filepath);
		Character p = GameManager.instance.PlayerCharacter;
		try
		{
			string json = File.ReadAllText(filepath);
			JsonUtility.FromJsonOverwrite(json, p);
		}
		catch (FileNotFoundException)
		{
			Debug.Log("saved char doesn't exist yet. i'll make one");
			SaveCharacter();
		}
		p.ResetTimers();
	}

	public static void SaveCharacter(Character stats)
	{
		string filepath = Application.persistentDataPath + "/character1.json";
		Debug.Log("writing " + filepath);
		File.WriteAllText(filepath, JsonUtility.ToJson(stats, true));
	}

	public static void SaveCharacter()
	{
		SaveCharacter(GameManager.instance.PlayerCharacter);
	}

	public static void LoadDungeon()
	{
		string filepath = Application.persistentDataPath + "/dungeon1.json";
		Debug.Log("reading " + filepath);
	}

	public static void SaveDungeon()
	{
		string filepath = Application.persistentDataPath + "/dungeon1.json";
		Debug.Log("writing " + filepath);
		File.WriteAllText(filepath, JsonUtility.ToJson(GameManager.instance.m_DungeonState, true));
	}

	public static void LoadSettings()
	{
		string filepath = Application.persistentDataPath + "/settings.json";
		Debug.Log("reading " + filepath);
		GameSettings s = GameManager.instance.Settings;
		try
		{
			string json = File.ReadAllText(filepath);
			JsonUtility.FromJsonOverwrite(json, s);
		}
		catch (FileNotFoundException)
		{
			Debug.Log("settings file doesn't exist yet. i'll make one");
			SaveSettings();
		}
	}

	public static void SaveSettings()
	{
		string filepath = Application.persistentDataPath + "/settings.json";
		Debug.Log("writing " + filepath);
		GameSettings s = GameManager.instance.Settings;
		File.WriteAllText(filepath, JsonUtility.ToJson(s, true));
	}
}
