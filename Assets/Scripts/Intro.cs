using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	public GameObject buttons;
	public Text progressText;
	public Image progressBar;

	private void Start()
	{
		//CanvasManager.DisableGameCanvas();
		Debug.Log("Application.streamingAssetsPath = '" + Application.streamingAssetsPath + "'");
		Debug.Log("Application.dataPath = '" + Application.dataPath + "'");
		buttons.SetActive(false);
	}

	void Update()
	{
		if (AssetLoader.allfilesfinished)
		{
			progressText.text = "ready";
			progressBar.fillAmount = 1f;
			buttons.SetActive(true);
		}
		else if (AssetLoader.inprogress)
		{
			progressText.text = string.Format("{0} {1:0.#}%", AssetLoader.filename, AssetLoader.progress * 100f);
			progressBar.fillAmount = AssetLoader.progress;
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
