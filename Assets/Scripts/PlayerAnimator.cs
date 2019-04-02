using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerAnimator : MonoBehaviour
{
	GameManager m_Manager;
	Player player;
	NavMeshAgent m_Agent;
	Animator m_Animator;
	readonly float maxspeed = 3.0f;
	GameObject SelectedTarget = null;

	void Start()
	{
		m_Manager = GameManager.instance;
		player = gameObject.GetComponentInParent<Player>();
		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		m_Agent.speed = maxspeed;
		m_Agent.angularSpeed = 500;
		m_Agent.stoppingDistance = 1.1f;
		m_Agent.updateRotation = true;
		// not tested - might be useful to enable stoppingDistance
		//m_Agent.stoppingDistance = 1.1f;
		//m_Agent.updateRotation = true;
		if (SceneManager.GetActiveScene().name == "Town")
		{
			m_Agent.speed = 2.0f;
		}
	}

	void Update()
	{
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / maxspeed;
		m_Animator = GetComponentInChildren<Animator>();//lets us change avatars from editor, but otherwise, bad place for this
		if (m_Animator) m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		if (SelectedTarget != null)
		{
			Interactable ia = SelectedTarget.gameObject.GetComponent<Interactable>();
			if (ia)
			{
				float distanceToTarget = Vector3.Distance(transform.position, SelectedTarget.transform.position);
				if (distanceToTarget < ia.radius)
				{
					Stop();
					Enemy e = SelectedTarget.gameObject.GetComponent<Enemy>();
					bool caninteract = (e != null) ? player.Stats.CanAttack() : true;
					if (caninteract)
					{
						bool didinteract = ia.Interact();
						if (e != null && didinteract) DoAttack();
					}
					SelectedTarget = null;
				}
				else
				{
					MoveTo(SelectedTarget.transform.position);
				}
			}
		}
		// adding a rigidbody to player caused some really weird and broken
		// movement, but we need it to trigger waypoints. this seems to fix it.
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}

	void MoveTo(Vector3 point)
	{
		m_Agent.stoppingDistance = 1.1f;
		m_Agent.SetDestination(point);
	}

	public void SetTarget(GameObject target)
	{
		SelectedTarget = target;
		float distanceToTarget = Vector3.Distance(SelectedTarget.transform.position, Scene_Manager.instance.player.transform.position);
		//Debug.Log("clicked on '" + target.name + "' distance = " + distanceToTarget.ToString("0.0"));
		if (distanceToTarget > 1.2)
		{
			MoveTo(SelectedTarget.transform.position);
		}
		else
		{
			player.Animator.Stop();
			player.Animator.SetDirection();
		}
	}

	/// <summary>
	/// doesn't move - just turns the player to face that direction
	/// </summary>
	public void SetDirection()
	{
		Ray ray = m_Manager.ActiveCamera.ScreenPointToRay(Input.mousePosition);
		LayerMask movementMask = LayerMask.GetMask("Ground");
		if (Physics.Raycast(ray, out RaycastHit hit, movementMask))
		{
			// looks ugly turning this suddenly, but good enough for now
			transform.LookAt(hit.point);
		}
	}

	/// <summary>
	/// try to move the player to this location
	/// </summary>
	public void SetDestination()
	{
		Ray ray = m_Manager.ActiveCamera.ScreenPointToRay(Input.mousePosition);
		LayerMask movementMask = LayerMask.GetMask("Ground");
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, movementMask))
		{
			//Interactable interactable = hit.collider.GetComponent<Interactable>();
			//if (interactable)
			//{
			//	Debug.Log(hit.transform.name + " is interactable");
			//}
			string hitname = hit.transform.name;
			//if (hit.transform.name.StartsWith("Floor"))
			//if (hit.transform.gameObject.layer == movementMask)
			//if ((movementMask & 1 << hit.transform.gameObject.layer) == 1 << hit.transform.gameObject.layer)
			// this is ugly - find a better solution
			if (hitname.StartsWith("Floor") || hitname.StartsWith("Stairs") || hitname == "Plane" || hitname == "Terrain")// || hitname == "Start" || hitname == "End")
			{
				SelectedTarget = null;
				MoveTo(hit.point);
			}
		}
	}

	public void Stop()
	{
		m_Agent.stoppingDistance = 0f;
		MoveTo(transform.position);
	}

	/// <summary>
	/// play the attack animation and sound
	/// </summary>
	public void DoAttack()
	{
		// this should probably time these things. maybe use an animator
		m_Animator.Play("Attack");
		m_Manager.m_SoundManager.PlaySound(SoundManager.Sounds.PlayerAttack1, FindObjectOfType<Player>().Stats.Class);
	}
}
