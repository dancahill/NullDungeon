using UnityEngine;

public class Interactable : MonoBehaviour
{
	[HideInInspector] public float radius = 1.1f;

	private void OnMouseOver()
	{
		FindObjectOfType<GameCanvas>().SetInfo(name);
	}

	private void OnMouseExit()
	{
		FindObjectOfType<GameCanvas>().SetInfo("");
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

	public virtual bool Interact()
	{
		Debug.Log("interacting with " + transform.name);
		return false;
	}
}
