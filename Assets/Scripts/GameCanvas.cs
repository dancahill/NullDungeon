using UnityEngine;
using UnityEngine.SceneManagement;
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
		m_Manager.Settings.CameraSkew = 0;
		CharacterPanel.SetActive(false);
		InventoryPanel.SetActive(false);
		BottomPanel.SetActive(true);
	}

	public void SetInfo(string message)
	{
		GameObject info = GameObject.Find("InfoBox");
		Text t = info.GetComponent<Text>();
		t.text = message.ToUpper();
	}

	public void OpenMenu()
	{
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ClickHeavy);
		GameSave.SaveGame();
		m_Manager.fader.FadeTo("MainMenu");

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
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.Click);
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
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.Click);
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
