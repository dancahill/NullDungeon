using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
	private Canvas m_Canvas;
	private CanvasScaler m_CanvasScaler;
	private GameObject m_Panel;
	private Image m_Image;
	float time = 1;

	void Awake()
	{
		gameObject.layer = 5;// UI

		m_Canvas = gameObject.AddComponent<Canvas>();
		m_CanvasScaler = gameObject.AddComponent<CanvasScaler>();

		m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		m_Canvas.sortingOrder = 999;

		m_Panel = new GameObject("Panel");
		m_Panel.transform.parent = gameObject.transform;
		m_Panel.AddComponent<CanvasRenderer>();
		m_Panel.layer = 5;// UI

		m_Image = m_Panel.AddComponent<Image>();
		m_Image.color = Color.black;

		RectTransform rt = m_Panel.GetComponent<RectTransform>();
		rt.localPosition = Vector3.zero;
		rt.anchorMin = new Vector2(0, 0);
		rt.anchorMax = new Vector2(1, 1);
	}

	private void Start()
	{
		StartCoroutine(FadeIn());
	}

	public void FadeTo(string scene)
	{
		//GameManager.Territory = "";
		Time.timeScale = 1;
		StartCoroutine(FadeOut(scene));
	}

	public void FadeTo(string scene, string territory)
	{
		//GameManager.Territory = territory;
		Time.timeScale = 1;
		StartCoroutine(FadeOut(scene));
	}

	IEnumerator FadeIn()
	{
		float t = time;

		//Time.timeScale = 0;
		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = t / time;
			//Debug.Log(string.Format("fading in - alpha = {0}", a));
			m_Image.color = new Color(0, 0, 0, a);
			yield return 0;
		}
		Time.timeScale = 1;
	}

	IEnumerator FadeOut(string scene)
	{
		float t = 0f;

		//Time.timeScale = 0;
		while (t < time)
		{
			t += Time.deltaTime;
			//curve.Evaluate(t);
			float a = t / time;
			//Debug.Log(string.Format("fading out - alpha = {0}", a));
			m_Image.color = new Color(0, 0, 0, a);
			yield return 0;
		}
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
	}
}
