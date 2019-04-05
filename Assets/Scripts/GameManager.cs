using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameSettings
{
	[NonSerialized] public float CameraSkew = 0;
	[NonSerialized] public bool FreshMeat = true;
	[NonSerialized] public bool NewInTown = true;
	public float CameraZoom = 0;
	public bool PlayMusic = true;
	public bool PlaySound = true;
}

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager instance;
	#endregion
	public GameSettings Settings;
	public Character PlayerCharacter;
	public DungeonState m_DungeonState;
	[HideInInspector] public SoundManager m_SoundManager;
	[HideInInspector] public SceneController sceneController;
	[HideInInspector] public Camera ActiveCamera;

	void Awake()
	{
		instance = this;
		if (SceneManager.GetActiveScene().name != "Persistent")
		{
			SceneManager.LoadScene("Persistent");
			return;
		}
		Settings = new GameSettings();
		GameSave.LoadSettings();
		PlayerCharacter = new Character(Character.CharacterClass.Warrior);
		sceneController = FindObjectOfType<SceneController>();
		if (!sceneController) throw new UnityException("Scene Controller missing. Make sure it exists in the Persistent scene.");
		if (sceneController.CurrentScene == "") sceneController.CurrentScene = "MainMenu";
	}

	void Start()
	{
		m_DungeonState = new DungeonState();
		m_SoundManager = gameObject.AddComponent<SoundManager>();
		m_SoundManager.PlayMusic();
	}

	public static Character GetPlayer()
	{
		return GameManager.instance.PlayerCharacter;
	}
}
