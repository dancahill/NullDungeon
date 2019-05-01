using UnityEngine;

// we need to attach this object to each animal on the object where the collider lives
// which is probably not at the top level of the prefab
[RequireComponent(typeof(MeshCollider))]
public class ColliderInteraction : Interactable
{
	Interactable target;

	private void Start()
	{
		target = transform.parent.GetComponent<Interactable>();
		if (!target) return;
		//if (!target) target = GetComponentInParent<Interactable>();
		overheadCanvas = target.overheadCanvas;
		text = target.text;
	}

	public override bool Interact()
	{
		if (!target)
		{
			Debug.LogWarning("no interactable found for " + name);
			return false;
		}
		return target.Interact();
	}

	//public override string Describe(RaycastHit hit)
	//{
	//	if (target) return target.Describe(hit);
	//	return "";
	//}
}
