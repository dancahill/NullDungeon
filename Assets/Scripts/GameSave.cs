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
		catch (FileNotFoundException ex)
		{
			Debug.Log("saved char doesn't exist yet. i'll make one");
			SaveGame();
		}
		p.Stats.ResetTimers();
	}

	public static void SaveGame()
	{
		string filepath = Application.persistentDataPath + "/character.json";
		Debug.Log("writing " + filepath);
		Player p = GameManager.instance.player.GetComponent<Player>();
		File.WriteAllText(filepath, JsonUtility.ToJson(p.Stats));
	}
}
