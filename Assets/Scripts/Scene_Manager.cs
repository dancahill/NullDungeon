﻿using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
	#region Singleton
	public static Scene_Manager instance;
	#endregion
	GameManager manager;
	GameSettings Settings;
	public GameObject player;

	[HideInInspector] public PlayerAnimator PlayerAnimator;

	SceneController sceneController;
	public bool NavMeshBaked = false;

	void Awake()
	{
		instance = this;
		if (SceneManager.GetActiveScene().name != "Persistent")
		{
			SceneManager.LoadScene("Persistent");
			return;
		}
		manager = FindObjectOfType<GameManager>();
		sceneController = manager.sceneController;
		NavMeshBaked = false;
		RebuildNavMesh();
		NavMeshBaked = true;
	}

	private void Start()
	{
		FindObjectOfType<CanvasManager>().SetActiveCanvas();
		string scene = SceneManager.GetActiveScene().name;
		if (scene == "GameOver")
		{
			GameManager.instance.m_SoundManager.PlayMusic();
			return;
		}
		// fix later

		Settings = manager.Settings;
		player = GameObject.Find("Player");

		if (scene != "MainMenu")
		{
			GameCanvas c = FindObjectOfType<GameCanvas>();
			c.OpenCharacterPanel(false);
			c.OpenInventoryPanel(false);

			if (Settings.NewInTown)
			{
				Settings.NewInTown = false;
				player.transform.position = new Vector3(37, 0, 12);
			}
			GameSave.LoadCharacter();
			if (SceneManager.GetActiveScene().name == "Town")
			{
				Light l = player.GetComponentInChildren<Light>();
				l.enabled = false;
			}
			if (player != null)
			{
				void setactive(string cclass)
				{
					GameObject warrior = player.transform.Find("Warrior").gameObject;
					GameObject rogue = player.transform.Find("Rogue").gameObject;
					warrior.SetActive(cclass == "Warrior");
					rogue.SetActive(cclass == "Rogue");
				}
				Player p = player.GetComponent<Player>();
				if (p.Stats.Class == CharacterStats.CharacterClass.Warrior)
				{
					// clean this later to avoid making a mess when adding more classes
					//GameObject warrior = player.transform.Find("Warrior").gameObject;
					//GameObject rogue = player.transform.Find("Rogue").gameObject;
					//warrior.SetActive(true);
					//rogue.SetActive(false);
					setactive("Warrior");
				}
				else if (p.Stats.Class == CharacterStats.CharacterClass.Rogue)
				{
					//GameObject warrior = player.transform.Find("Warrior").gameObject;
					//GameObject rogue = player.transform.Find("Rogue").gameObject;
					//warrior.SetActive(false);
					//rogue.SetActive(true);
					setactive("Rogue");
				}
			}
			PlayerAnimator = player.GetComponent<PlayerAnimator>();
		}

		if (player)
		{
			Player p = player.GetComponent<Player>();
			if (p.Stats.Life < 1) p.Stats.Life = p.Stats.BaseLife;
		}
		manager.m_SoundManager.PlayMusic();
		if (scene == "Dungeon1" && Settings.FreshMeat)
		{
			Settings.FreshMeat = false;
			manager.m_SoundManager.PlaySound(SoundManager.Sounds.FreshMeat);
		}
	}

	void LateUpdate()
	{
		RepositionCamera();
	}

	private void OnEnable()
	{
		sceneController.BeforeSceneUnload += Save;
		sceneController.AfterSceneLoad += Load;
	}

	private void OnDisable()
	{
		sceneController.BeforeSceneUnload -= Save;
		sceneController.AfterSceneLoad -= Load;
	}

	private void Load()
	{
		manager.m_DungeonState.LoadState(sceneController.CurrentScene);
	}

	private void Save()
	{
		manager.m_DungeonState.SaveState(sceneController.CurrentScene);
	}

	void RebuildNavMesh()
	{
		GameObject env = GameObject.Find("Environment");
		if (env != null)
		{
			NavMeshSurface nms = env.AddComponent<NavMeshSurface>();
			nms.layerMask = 1 << LayerMask.NameToLayer("Ground");
			nms.BuildNavMesh();
		}
	}

	void RepositionCamera()
	{
		string scene = SceneManager.GetActiveScene().name;
		if (scene != "MainMenu" && scene != "GameOver")
		{
			manager.ActiveCamera.transform.position = player.transform.position + new Vector3(4 - Settings.CameraZoom, 5 - Settings.CameraZoom, 4 - Settings.CameraZoom);
			manager.ActiveCamera.transform.LookAt(player.transform.position + new Vector3(0, 0.1f, 0));
			if (Settings.CameraSkew != 0) manager.ActiveCamera.transform.Translate(Vector3.right * Settings.CameraSkew * 3, Space.Self);
		}
	}
}