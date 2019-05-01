using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
	public void FootStep()
	{
		SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.Footstep1);
	}
}
