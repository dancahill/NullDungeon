using UnityEngine;
using UnityEngine.SceneManagement;

public class WayPoint : MonoBehaviour
{
	bool hit = false;

	private void OnMouseOver()
	{
		FindObjectOfType<GameCanvas>().SetInfo(name + " Waypoint");
	}

	private void OnMouseExit()
	{
		FindObjectOfType<GameCanvas>().SetInfo("");
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (hit) return;
		hit = true;
		string currentscene = SceneManager.GetActiveScene().name;
		//Debug.Log("collision with " + name + " point by " + collision.gameObject.name);
		if (name == "Start")
		{
			if (currentscene == "Dungeon2")
			{
				GameSave.SaveCharacter();
				GameManager.instance.sceneController.FadeAndLoadScene("Dungeon1");
			}
			else
			{
				GameSave.SaveCharacter();
				GameManager.instance.sceneController.FadeAndLoadScene("Town");
			}
		}
		else if (name == "End")
		{
			if (currentscene == "Town")
			{
				GameSave.SaveCharacter();
				GameManager.instance.sceneController.FadeAndLoadScene("Dungeon1");
			}
			else if (currentscene == "Dungeon1")
			{
				GameSave.SaveCharacter();
				GameManager.instance.sceneController.FadeAndLoadScene("Dungeon2");
			}
			else
			{
				GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CantDoThat, FindObjectOfType<Player>().Stats.Class);
				Debug.Log("this is as deep as we go");
				hit = false;
			}
		}
	}
}
