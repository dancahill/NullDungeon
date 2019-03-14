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

	public void OpenCharacterPanel()
	{
		CharacterPanel.SetActive(!CharacterPanel.activeSelf);
		//RectTransform rta = gameObject.GetComponent<RectTransform>();
		//RectTransform rtc = CharacterPanel.GetComponent<RectTransform>();
		//RectTransform rtb = BottomPanel.GetComponent<RectTransform>();
		//Debug.Log(string.Format("game {0},{1} {2},{3}", Screen.width, Screen.height, rta.rect.width, rta.rect.height));
		//Debug.Log(string.Format("char {0},{1} {2},{3}", Screen.width, Screen.height, rtc.rect.width, rtc.rect.height));
		//Debug.Log(string.Format("bott {0},{1} {2},{3}", Screen.width, Screen.height, rtb.rect.width, rtb.rect.height));
		//rtc.sizeDelta = new Vector2(rtc.rect.height, rta.rect.height - rtb.rect.height);
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
