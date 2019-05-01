using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public Canvas overheadCanvas;
	public Text text;
	[HideInInspector] public float radius = 1.2f;
	bool mouseover;

	protected virtual void Awake()
	{
		if (overheadCanvas) overheadCanvas.gameObject.SetActive(false);
	}

	protected virtual void Update()
	{
		if (mouseover || (GameManager.instance.Settings.ShowItemsOnGround && GetComponent<Loot>()))
		{
			EnableCanvas();
		}
		else
		{
			DisableCanvas();
		}
	}

	private void OnMouseOver()
	{
		//Debug.Log("mouse over " + name);
		mouseover = true;
		EnableCanvas();
		FindObjectOfType<GameCanvas>().SetInfo(name);
	}

	private void OnMouseExit()
	{
		//Debug.Log("mouse leaving " + name);
		mouseover = false;
		DisableCanvas();
		FindObjectOfType<GameCanvas>().SetInfo("");
	}

	void OnMouseDown()
	{
		GameObject player = GameObject.Find("Player");
		player.GetComponent<PlayerAnimator>().SetTarget(transform.gameObject);
	}

	public void EnableCanvas()
	{
		if (overheadCanvas)
		{
			overheadCanvas.gameObject.SetActive(true);
			AlignCanvas();
		}
	}

	public void DisableCanvas()
	{
		if (overheadCanvas) overheadCanvas.gameObject.SetActive(false);
	}

	public void AlignCanvas()
	{
		if (!overheadCanvas || !overheadCanvas.isActiveAndEnabled) return;
		Camera cam = GameManager.instance.ActiveCamera;
		overheadCanvas.transform.LookAt(overheadCanvas.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
	}

	public virtual bool Interact()
	{
		//Debug.Log("interacting with " + transform.name);
		return false;
	}
}
