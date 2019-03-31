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
	public GameSettings()
	{
		//GameSave.LoadSettings();
	}
}

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager instance;
	#endregion
	public GameSettings Settings;
	public CharacterStats PlayerStats;
	public DungeonState m_DungeonState;
	[HideInInspector] public SoundManager m_SoundManager;
	[HideInInspector] public SceneController sceneController;
	[HideInInspector] public Camera ActiveCamera;

	void Awake()
	{
		instance = this;
		Settings = new GameSettings();
		GameSave.LoadSettings();
		if (SceneManager.GetActiveScene().name != "Persistent")
		{
			SceneManager.LoadScene("Persistent");
			return;
		}
		PlayerStats = new CharacterStats(CharacterStats.CharacterClass.Warrior);
		sceneController = FindObjectOfType<SceneController>();
		if (!sceneController) throw new UnityException("Scene Controller missing. Make sure it exists in the Persistent scene.");
		if (sceneController.CurrentScene == "") sceneController.CurrentScene = "MainMenu";
		SetCamera();
	}

	void Start()
	{
		m_DungeonState = new DungeonState();
		m_SoundManager = gameObject.AddComponent<SoundManager>();
		m_SoundManager.PlayMusic();
	}

	void SetCamera()
	{
		if (sceneController.CurrentScene == "MainMenu")
		{
			GameObject c = GameObject.Find("Cameras/MainMenu");
			c.SetActive(true);
			ActiveCamera = c.GetComponent<Camera>();
			c = GameObject.Find("Cameras/Game");
			c.SetActive(false);
		}
		else
		{
			GameObject c = GameObject.Find("Cameras/MainMenu");
			c.SetActive(false);
			c = GameObject.Find("Cameras/Game");
			c.SetActive(true);
			ActiveCamera = c.GetComponent<Camera>();
		}
	}
}
