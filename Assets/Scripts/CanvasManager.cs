using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	public GameObject MainMenuCanvas;
	public GameObject GameCanvas;

	public void SetActiveCanvas()
	{
		if (SceneController.GetActiveSceneName() == "MainMenu")
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
