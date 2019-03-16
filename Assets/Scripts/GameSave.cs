using System.IO;
using UnityEngine;

public class GameSave
{
	//https://docs.unity3d.com/Manual/JSONSerialization.html
	public static void LoadGame()
	{
		string filepath = Application.persistentDataPath + "/character.json";
		Debug.Log("reading " + filepath);
		Player p = GameManager.instance.player.GetComponent<Player>();
		try
		{
			string json = File.ReadAllText(filepath);
			JsonUtility.FromJsonOverwrite(json, p.Stats);
		}
		catch (FileNotFoundException)
		{
			Debug.Log("saved char doesn't exist yet. i'll make one");
			SaveGame();
		}
		p.Stats.ResetTimers();
	}

	public static void SaveGame(CharacterStats stats)
	{
		string filepath = Application.persistentDataPath + "/character.json";
		Debug.Log("writing " + filepath);
		File.WriteAllText(filepath, JsonUtility.ToJson(stats, true));
	}

	public static void SaveGame()
	{
		string filepath = Application.persistentDataPath + "/character.json";
		Debug.Log("writing " + filepath);
		Player p = GameManager.instance.player.GetComponent<Player>();
		File.WriteAllText(filepath, JsonUtility.ToJson(p.Stats, true));
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
