using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
	GameManager m_Manager;
	public GameObject CharacterPanel;
	public GameObject InventoryPanel;
	public GameObject BottomPanel;

	private void Start()
	{
		m_Manager = GameManager.instance;
	}


	public void SetInfo(string message)
	{
		GameObject info = GameObject.Find("InfoBox");
		Text t = info.GetComponent<Text>();
		t.text = message;
	}

	public void OpenMenu()
	{
		SetInfo("Quitting");
		Application.Quit();
	}

	public void OpenCharacterPanel()
	{
		CharacterPanel.SetActive(!CharacterPanel.activeSelf);
		AdjustCameraSkew();
	}

	public void OpenInventoryPanel()
	{
		InventoryPanel.SetActive(!InventoryPanel.activeSelf);
		AdjustCameraSkew();
	}

	void AdjustCameraSkew()
	{
		if (CharacterPanel.activeSelf && !InventoryPanel.activeSelf) m_Manager.Settings.CameraSkew = -1;
		else if (!CharacterPanel.activeSelf && InventoryPanel.activeSelf) m_Manager.Settings.CameraSkew = 1;
		else m_Manager.Settings.CameraSkew = 0;
	}
}
