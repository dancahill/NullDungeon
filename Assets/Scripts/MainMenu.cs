using UnityEngine;

public class MainMenu : MonoBehaviour
{
	GameManager m_Manager;

	void Start()
	{
		m_Manager = GameManager.instance;
	}

	public void MenuContinueGame()
	{
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ClickHeavy);
		m_Manager.Settings.NewInTown = true;
		m_Manager.sceneController.FadeAndLoadScene("Town");
	}

	public void MenuNewGame()
	{
		GameSave.SaveCharacter(new Character(Character.CharacterClass.Warrior));
		MenuContinueGame();
	}

	public void MenuExit()
	{
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.ClickHeavy);
		Application.Quit();
		m_Manager.sceneController.FadeAndLoadScene("GameOver");
	}
}
