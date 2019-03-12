using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeCheck : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		Vector3 size = mr.bounds.size;
		Debug.Log(string.Format("size={0},{1},{2}", size.x, size.y, size.z));
		size.x = 2;
		size.z = 2;
	}
}
