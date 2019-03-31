using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	public GameObject MainMenuCanvas;
	public GameObject GameCanvas;

	SceneController sceneController;

	void Start()
	{
		sceneController = GameManager.instance.sceneController;
	}

	public void SetActiveCanvas()
	{
		// can't Find() objects if they're disabled
		//GameObject mcanvas = GameObject.Find("Canvas/MainMenu");
		//GameObject gcanvas = GameObject.Find("Canvas/Game");
		if (sceneController.CurrentScene == "" || sceneController.CurrentScene == "MainMenu")
		{
			MainMenuCanvas.SetActive(true);
			GameCanvas.SetActive(false);
		}
		else
		{
			//Settings.CameraSkew = 0;
			MainMenuCanvas.SetActive(false);
			GameCanvas.SetActive(true);
			GameCanvas c = FindObjectOfType<GameCanvas>();
			c.OpenCharacterPanel(false);
			c.OpenInventoryPanel(false);
			c.BottomPanel.SetActive(true);
		}
	}
}
