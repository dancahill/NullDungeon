using UnityEngine;

public class Interactable : MonoBehaviour
{
	public Canvas overheadCanvas;
	[HideInInspector] public float radius = 1.1f;

	protected virtual void Awake()
	{
		if (overheadCanvas) overheadCanvas.gameObject.SetActive(false);
	}

	protected virtual void Update()
	{
		AlignCanvas();
	}

	private void OnMouseOver()
	{
		Debug.Log("mouse over " + name);
		EnableCanvas();
	}

	private void OnMouseExit()
	{
		Debug.Log("mouse leaving " + name);
		DisableCanvas();
	}

	void OnMouseDown()
	{
		GameObject player = GameObject.Find("Player");
		player.GetComponent<PlayerAnimator>().SetTarget(transform.gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, Enemy.ProvokeRadius);
	}

	public void EnableCanvas()
	{
		FindObjectOfType<GameCanvas>().SetInfo(name);
		if (overheadCanvas) overheadCanvas.gameObject.SetActive(true);
	}

	public void DisableCanvas()
	{
		FindObjectOfType<GameCanvas>().SetInfo("");
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
		Debug.Log("interacting with " + transform.name);
		return false;
	}
}
