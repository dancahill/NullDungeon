﻿using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
	[NonSerialized] public float CameraSkew = 0;
	[NonSerialized] public bool FreshMeat = true;
	[NonSerialized] public bool NewInTown = true;
	[NonSerialized] public bool ShowItemsOnGround = false;
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
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Persistent")
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Persistent");
			return;
		}
		Settings = new GameSettings();
		GameSave.LoadSettings();
		PlayerCharacter = new Character(Character.CharacterClass.NPC);
		sceneController = FindObjectOfType<SceneController>();
		if (!sceneController) throw new UnityException("Scene Controller missing. Make sure it exists in the Persistent scene.");
		if (sceneController.CurrentScene == "") sceneController.CurrentScene = "Intro";
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