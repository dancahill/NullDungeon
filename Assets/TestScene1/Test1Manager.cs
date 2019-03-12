using UnityEngine;
using UnityEngine.AI;

public class Test1Manager : MonoBehaviour
{
	public GameObject player;
	public GameObject end;
	public float CameraZoom = 0;
	AudioSource m_Audio;

	void Awake()
	{
		RebuildNavMesh();
	}

	void Start()
	{
		StartGameMusic();
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
		AudioClip clip = (AudioClip)Resources.Load("Diablo/03 - Dungeon");
		if (!m_Audio) m_Audio = gameObject.AddComponent<AudioSource>();
		if (m_Audio) m_Audio.PlayOneShot(clip);
	}

	void RepositionCamera()
	{
		Camera.main.transform.position = player.transform.position + new Vector3(4 - CameraZoom, 5 - CameraZoom, 4 - CameraZoom);
		Camera.main.transform.LookAt(player.transform);
	}
}
