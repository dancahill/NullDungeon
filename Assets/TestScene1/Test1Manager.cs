using UnityEngine;
using UnityEngine.AI;

public class Test1Manager : MonoBehaviour
{
	public GameObject player;
	public GameObject end;

	void Awake()
	{
		GameObject env = GameObject.Find("Environment");
		NavMeshSurface nms = env.AddComponent<NavMeshSurface>();
		nms.layerMask = 1 << LayerMask.NameToLayer("Environment");
		nms.BuildNavMesh();
	}

	private void Update()
	{
		Camera.main.transform.LookAt(player.transform);
	}
}
