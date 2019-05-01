using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
	public static SceneManager instance;
	GameManager manager;
	GameSettings Settings;
	public GameObject player;
	SceneController sceneController;

	void Awake()
	{
		instance = this;
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Persistent")
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Persistent");
			return;
		}
		SetCamera();
		manager = FindObjectOfType<GameManager>();
		sceneController = manager.sceneController;
		RebuildNavMesh();
	}

	private void Start()
	{
		FindObjectOfType<CanvasController>().SetActiveCanvas();
		string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		if (scene == "GameOver")
		{
			GameManager.instance.m_SoundManager.PlayMusic();
			Application.Quit();
			return;
		}
		// fix later

		Settings = manager.Settings;
		player = GameObject.Find("Player");

		manager.m_SoundManager.PlayMusic();
		if (scene != "Intro" && scene != "MainMenu")
		{
			GameCanvas c = FindObjectOfType<GameCanvas>();
			c.OpenCharacterPanel(false);
			c.OpenInventoryPanel(false);
			GameSave.LoadCharacter();
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Town" && Settings.NewInTown)
			{
				Settings.NewInTown = false;
				manager.PlayerCharacter.Life = manager.PlayerCharacter.BaseLife;
				player.transform.position = new Vector3(15.1f, 0.1f, 23.4f);
			}
			if (scene == "Dungeon1" && Settings.FreshMeat)
			{
				Settings.FreshMeat = false;
				manager.m_SoundManager.PlaySound(SoundManager.Sounds.FreshMeat);
			}
		}
		if (player)
		{
			Player p = player.GetComponent<Player>();
			if (p.Stats.Life < 1) p.Stats.Life = p.Stats.BaseLife;
		}
	}

	void LateUpdate()
	{
		RepositionCamera();
		RepositionMinimapCamera();
	}

	private void OnEnable()
	{
		if (!sceneController) return;
		sceneController.BeforeSceneUnload += Save;
		sceneController.AfterSceneLoad += Load;
	}

	private void OnDisable()
	{
		if (!sceneController) return;
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
			nms.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
			nms.BuildNavMesh();
		}
	}

	void SetCamera()
	{
		GameManager.instance.ActiveCamera = Camera.main;
	}

	void RepositionCamera()
	{
		GameObject player = GameObject.Find("Player");
		//string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		//if (scene != "Intro" && scene != "MainMenu" && scene != "GameOver")
		if (player)
		{
			//manager.ActiveCamera.transform.position = player.transform.position + new Vector3(4 - Settings.CameraZoom, 5 - Settings.CameraZoom, 4 - Settings.CameraZoom);
			//manager.ActiveCamera.transform.LookAt(player.transform.position + new Vector3(0, 0.1f, 0));
			//if (Settings.CameraSkew != 0) manager.ActiveCamera.transform.Translate(Vector3.right * Settings.CameraSkew * 3, Space.Self);

			manager.ActiveCamera.transform.position = player.transform.TransformPoint(0, 1, 0);
			manager.ActiveCamera.transform.rotation = Quaternion.Euler(30f, -135f, 0f);
			manager.ActiveCamera.orthographicSize = 3f - Settings.CameraZoom;

			if (Settings.CameraSkew != 0) manager.ActiveCamera.transform.Translate(Vector3.right * Settings.CameraSkew * 3, Space.Self);
		}
	}

	void RepositionMinimapCamera()
	{
		if (!player) return;
		Transform minicam = player.transform.Find("Minimap Camera");
		if (!minicam) return;
		minicam.position = player.transform.position + new Vector3(20, 21, 20);
		minicam.LookAt(player.transform.position + new Vector3(0, 0.1f, 0));
	}
}
