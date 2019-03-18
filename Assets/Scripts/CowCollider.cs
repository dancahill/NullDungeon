using UnityEngine;

public class CowCollider : MonoBehaviour
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
		GameManager.instance.m_SoundManager.PlaySound(SoundManager.Sounds.CowAlright);
	}
}
