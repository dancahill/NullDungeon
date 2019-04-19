using UnityEngine;

public class Intro : MonoBehaviour
{
	private void Start()
	{
		//CanvasManager.DisableGameCanvas();
	}

	void Update()
	{
		if (Time.time > 10)
		{
			GameManager.instance.sceneController.FadeAndLoadScene("MainMenu");
		}
	}

	public void VisitNullLogic()
	{
		Application.OpenURL("https://nulllogic.ca/unity/");
	}

	public void LoadMap()
	{
		GameManager.instance.sceneController.FadeAndLoadScene("MainMenu");
	}

	public void LoadTestMap()
	{
		//GameManager.instance.sceneController.FadeAndLoadScene("TestMap1");
	}
}
