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
	public static GameSettings settingsinstance = new GameSettings();
	public static GameManager instance;
	//static string m_StartScene = "";
	#endregion

	public GameSettings Settings;

	public CharacterStats PlayerStats = new CharacterStats(CharacterStats.CharacterClass.Warrior);

	public DungeonState m_DungeonState;

	//[HideInInspector] public GameObject player;
	//[HideInInspector] public GameCanvas Canvas;
	[HideInInspector] public GameObject SelectedTarget = null;
	[HideInInspector] public SoundManager m_SoundManager;

	[HideInInspector] public SceneController sceneController;
	[HideInInspector] public Camera ActiveCamera;

	//[HideInInspector] public GameObject faderObject;
	//[HideInInspector] public SceneFader fader;

	void Awake()
	{
		instance = this;
		Settings = settingsinstance;

		//if (m_StartScene == "")
		//{
		//	if (SceneManager.GetActiveScene().name != "MainMenu")
		//	{
		//		//Debug.Log("you should start in town");
		//		SceneManager.LoadScene("MainMenu");
		//		return;
		//	}
		//	m_StartScene = "MainMenu";
		//}

		if (SceneManager.GetActiveScene().name != "Persistent")
		{
			//Debug.Log("you should start in town");
			SceneManager.LoadScene("Persistent");
			return;
		}

		sceneController = FindObjectOfType<SceneController>();
		if (!sceneController) throw new UnityException("Scene Controller missing. Make sure it exists in the Persistent scene.");
		if (sceneController.CurrentScene == "") sceneController.CurrentScene = "MainMenu";
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

		GameSave.LoadSettings();
		//player = GameObject.Find("Player");
		//player = new GameObject("Player");
		///player.AddComponent<Player>();
		//player.AddComponent<CharacterStats>();
		//Stats = new CharacterStats(CharacterStats.CharacterClass.Warrior);



		// add a fader
		//faderObject = new GameObject("SceneFader");
		//fader = faderObject.AddComponent<SceneFader>();
		//fader = FindObjectOfType<SceneFader>();
	}

	void Start()
	{
		m_DungeonState = new DungeonState();
		m_SoundManager = gameObject.AddComponent<SoundManager>();
		m_SoundManager.PlayMusic();
	}
}
