﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class WayPoint : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		string currentscene = SceneManager.GetActiveScene().name;
		Debug.Log("collision with " + name + " point by " + collision.gameObject.name);
		if (name == "Start")
		{
			if (currentscene == "Town")
			{
				//SceneManager.LoadScene("Dungeon1");
			}
			else
			{
				Debug.Log("fleeing to town");
				SceneManager.LoadScene("Town");
			}
		}
		else if (name == "End")
		{
			if (currentscene == "Town")
			{
				Debug.Log("descending into madness");
				SceneManager.LoadScene("Dungeon1");
			}
			else
			{
				Debug.Log("this is as deep as we go");
			}
		}
	}
}