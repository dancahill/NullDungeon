using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
	[HideInInspector]
	public GameManager Manager;
	[HideInInspector]
	public PlayerAnimator Animator;
	[HideInInspector]
	public PlayerInput Input;

	private void Start()
	{
		Manager = GameManager.instance;
		Animator = GetComponent<PlayerAnimator>();
		Input = GetComponent<PlayerInput>();
	}
}
