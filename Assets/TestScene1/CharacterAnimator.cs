using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
	const float locomotionAnimationSmoothTime = .1f;
	NavMeshAgent agent;
	Animator animator;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		Test1Manager tm = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
		agent.SetDestination(tm.end.transform.position);
	}

	void Update()
	{
		float speedPercent = agent.velocity.magnitude / agent.speed;
		animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		if (Input.GetMouseButtonDown(0))
		{
			Camera cam = Camera.main;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			LayerMask movementMask = LayerMask.GetMask("Environment");
			if (Physics.Raycast(ray, out hit, movementMask))
			{
				agent.SetDestination(hit.point);
			}
		}
	}
}
