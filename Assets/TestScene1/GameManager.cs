using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSettings
{
	public float CameraZoom;
}

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameSettings settingsinstance = new GameSettings();
	public static GameManager instance;
	#endregion
	public GameSettings Settings;
	public GameObject player;
	//GameObject end;
	AudioSource m_Audio;
	[HideInInspector]
	public GameObject SelectedTarget = null;
	[HideInInspector]
	public PlayerAnimator PlayerAnimator;

	void Awake()
	{
		instance = this;
		Settings = settingsinstance;
		player = GameObject.Find("Player");
		RebuildNavMesh();
	}

	void Start()
	{
		StartGameMusic();
		PlayerAnimator = player.GetComponent<PlayerAnimator>();
	}

	void Update()
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

	void StartGameMusic()
	{
		//https://downloads.khinsider.com/game-soundtracks/album/diablo-the-music-of-1996-2011-diablo-15-year-anniversary
		if (!m_Audio) m_Audio = gameObject.AddComponent<AudioSource>();
		if (m_Audio)
		{
			string currentscene = SceneManager.GetActiveScene().name;
			AudioClip clip = (AudioClip)Resources.Load(currentscene == "Town" ? "Diablo/02 - Tristram" : "Diablo/03 - Dungeon");
			m_Audio.loop = true;
			m_Audio.PlayOneShot(clip);
		}
	}

	void RepositionCamera()
	{
		Camera.main.transform.position = player.transform.position + new Vector3(4 - Settings.CameraZoom, 5 - Settings.CameraZoom, 4 - Settings.CameraZoom);
		Camera.main.transform.LookAt(player.transform);
	}
}
