using UnityEngine;

public class TownNPC : MonoBehaviour
{
	void OnMouseOver()
	{
		FindObjectOfType<GameCanvas>().SetInfo(name);
	}

	void OnMouseExit()
	{
		FindObjectOfType<GameCanvas>().SetInfo("");
	}

	void OnMouseDown()
	{
		if (name == "Deckard Cain")
		{
			GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CainHello);
		}
	}
}
