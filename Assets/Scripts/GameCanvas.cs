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
		CharacterPanel.SetActive(false);
		InventoryPanel.SetActive(false);
		BottomPanel.SetActive(true);
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

	public bool IsCharacterPanelOpen()
	{
		return CharacterPanel.activeSelf;
	}

	public void OpenCharacterPanel(bool state)
	{
		CharacterPanel.SetActive(state);
		AdjustCameraSkew();
	}

	public void ToggleCharacterPanel()
	{
		CharacterPanel.SetActive(!CharacterPanel.activeSelf);
		AdjustCameraSkew();
	}

	public bool IsInventoryPanelOpen()
	{
		return InventoryPanel.activeSelf;
	}

	public void OpenInventoryPanel(bool state)
	{
		InventoryPanel.SetActive(state);
		AdjustCameraSkew();
	}

	public void ToggleInventoryPanel()
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
