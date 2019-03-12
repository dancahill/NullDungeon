using UnityEngine;

public class SizeCheck : MonoBehaviour
{
	void Update()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		if (mr == null) return;
		Vector3 size = mr.bounds.size;
		Debug.Log(string.Format("size={0},{1},{2}", size.x, size.y, size.z));
	}
}
