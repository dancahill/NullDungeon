using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
	void Start()
	{
	}

	void Update()
	{
	}

	public void SetInfo(string message)
	{
		GameObject info = GameObject.Find("InfoBox");
		Text t = info.GetComponent<Text>();
		t.text = message;
	}
}
