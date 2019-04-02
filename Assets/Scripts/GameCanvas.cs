using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
	GameManager m_Manager;
	SceneController sceneController;
	public GameObject CharacterPanel;
	public GameObject InventoryPanel;
	public GameObject BottomPanel;
	public Image healthBar;

	void Awake()
	{
		sceneController = FindObjectOfType<SceneController>();
		if (!sceneController) throw new UnityException("Scene Controller missing. Make sure it exists in the Persistent scene.");
		return;
	}

	private void Start()
	{
		m_Manager = GameManager.instance;
	}

	private void Update()
	{
		healthBar.fillAmount = (float)m_Manager.PlayerCharacter.Life / (float)m_Manager.PlayerCharacter.BaseLife;
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
		GameSave.SaveCharacter();
		m_Manager.sceneController.FadeAndLoadScene("MainMenu");

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
		else GameManager.instance.Settings.CameraSkew = 0;
	}
}
