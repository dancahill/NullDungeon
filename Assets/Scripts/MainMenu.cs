using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	GameManager m_Manager;

	void Start()
	{
		m_Manager = GameManager.instance;
		GameSave.LoadSettings();
	}

	public void MenuContinueGame()
	{
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ClickHeavy);
		SceneManager.LoadScene("Town");
	}

	public void MenuNewGame()
	{
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ClickHeavy);
		GameSave.SaveGame(new CharacterStats(CharacterStats.CharacterClass.Warrior));
		SceneManager.LoadScene("Town");
	}

	public void MenuExit()
	{
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ClickHeavy);
		Debug.Log("Application.Quit();");
		Application.Quit();
	}
}
