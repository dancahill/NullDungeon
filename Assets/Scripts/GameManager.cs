using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSettings
{
	public float CameraZoom;
	public float CameraSkew;
}

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameSettings settingsinstance = new GameSettings();
	public static GameManager instance;
	static string m_StartScene = "";
	static bool m_NewInTown = true;
	#endregion

	public GameSettings Settings;
	public GameObject player;
	public GameCanvas Canvas;
	[HideInInspector]
	public GameObject SelectedTarget = null;
	[HideInInspector]
	public PlayerAnimator PlayerAnimator;

	public SoundManager m_SoundManager;

	void Awake()
	{
		if (m_StartScene == "")
		{
			if (SceneManager.GetActiveScene().name != "Town")
			{
				Debug.Log("you should start in town");
				SceneManager.LoadScene("Town");
				return;
			}
			m_StartScene = "Town";
		}
		instance = this;
		Settings = settingsinstance;
		player = GameObject.Find("Player");
		RebuildNavMesh();
	}

	void Start()
	{
		if (m_NewInTown)
		{
			m_NewInTown = false;
			player.transform.position = new Vector3(37, 0, 12);
		}
		if (SceneManager.GetActiveScene().name == "Town")
		{
			Light l = player.GetComponentInChildren<Light>();
			l.enabled = false;
		}
		PlayerAnimator = player.GetComponent<PlayerAnimator>();
		m_SoundManager = gameObject.AddComponent<SoundManager>();
		m_SoundManager.PlayMusic();
	}

	void LateUpdate()
	{
		RepositionCamera();
	}

	void RebuildNavMesh()
	{
		GameObject env = GameObject.Find("Environment");
		NavMeshSurface nms = env.AddComponent<NavMeshSurface>();
		nms.layerMask = 1 << LayerMask.NameToLayer("Ground");
		nms.BuildNavMesh();
	}

	void RepositionCamera()
	{
		Camera.main.transform.position = player.transform.position + new Vector3(4 - Settings.CameraZoom, 5 - Settings.CameraZoom, 4 - Settings.CameraZoom);
		Camera.main.transform.LookAt(player.transform.position + new Vector3(0, 0.1f, 0));
		if (Settings.CameraSkew != 0) Camera.main.transform.Translate(Vector3.right * Settings.CameraSkew * 3, Space.Self);
	}
}
