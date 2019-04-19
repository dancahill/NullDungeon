using UnityEngine;

public class CanvasController : MonoBehaviour
{
	public GameObject MainMenuCanvas;
	public GameObject GameCanvas;

	public void SetActiveCanvas()
	{
		if (SceneController.GetActiveSceneName() == "Intro")
		{
			MainMenuCanvas.SetActive(false);
			GameCanvas.SetActive(false);
		}
		else if (SceneController.GetActiveSceneName() == "MainMenu")
		{
			MainMenuCanvas.SetActive(true);
			GameCanvas.SetActive(false);
		}
		else
		{
			MainMenuCanvas.SetActive(false);
			GameCanvas.SetActive(true);
			GameCanvas c = FindObjectOfType<GameCanvas>();
			c.OpenCharacterPanel(false);
			c.OpenInventoryPanel(false);
			c.BottomPanel.SetActive(true);
		}
	}
}
