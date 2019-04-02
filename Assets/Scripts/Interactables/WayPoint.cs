using UnityEngine;

public class WayPoint : Interactable
{
	public override bool Interact()
	{
		base.Interact();
		string currentscene = SceneController.GetActiveSceneName();
		GameSave.SaveCharacter();
		if (name == "Start")
		{
			if (currentscene == "Dungeon2")
				GameManager.instance.sceneController.FadeAndLoadScene("Dungeon1");
			else
				GameManager.instance.sceneController.FadeAndLoadScene("Town");
		}
		else if (name == "End")
		{
			if (currentscene == "Town")
				GameManager.instance.sceneController.FadeAndLoadScene("Dungeon1");
			else if (currentscene == "Dungeon1")
				GameManager.instance.sceneController.FadeAndLoadScene("Dungeon2");
			else
				GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CantDoThat, FindObjectOfType<Player>().Stats.Class);
		}
		return true;
	}
}
