using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
	InventorySlot slot;

	private void Awake()
	{
		slot = GetComponentInParent<InventorySlot>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			slot.UseItem();
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			slot.ConsumeItem();
		}
	}
}
