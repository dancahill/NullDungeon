using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
	public CharacterStats Stats;
	[HideInInspector] public GameManager Manager;
	[HideInInspector] public PlayerAnimator Animator;
	[HideInInspector] public PlayerInput Input;

	private void Start()
	{
		Stats = new CharacterStats(CharacterStats.CharacterClass.Warrior);
		Manager = GameManager.instance;
		Animator = GetComponent<PlayerAnimator>();
		Input = GetComponent<PlayerInput>();
	}
}
