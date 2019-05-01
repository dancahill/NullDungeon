using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
	GameManager m_Manager;
	Player player;
	NavMeshAgent m_Agent;
	Animator m_Animator;
	readonly float maxspeed = 3.0f;
	public GameObject SelectedTarget = null;

	void Start()
	{
		m_Manager = GameManager.instance;
		player = gameObject.GetComponentInParent<Player>();
		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		m_Agent.autoBraking = false;
		m_Agent.speed = maxspeed;
		m_Agent.angularSpeed = 720;
		m_Agent.acceleration = 100;
		if (SceneController.GetActiveSceneName() == "Town")
		{
			m_Agent.speed = 2.0f;
		}
	}

	void Update()
	{
		//lets us change avatars from editor, but otherwise, bad place for this
		m_Animator = GetComponentInChildren<Animator>();
		float speed = GetSpeed();
		if (m_Agent.hasPath && m_Agent.remainingDistance < 0.2f)
		{
			m_Agent.ResetPath();
		}
		if (m_Animator)
		{
			bool intown = SceneController.GetActiveSceneName() == "Town";
			m_Animator.SetBool("IsWalking", intown && speed > .1f);
			m_Animator.SetBool("IsRunning", !intown && speed > .1f);
		}
		if (SelectedTarget)
		{
			if (!TryToInteract() && SelectedTarget) MoveTo(SelectedTarget.transform.position);
		}
		// adding a rigidbody to player caused some really weird and broken
		// movement, but we need it to trigger waypoints. this seems to fix it.
		// ! i think adding 'is kinematic' fixed this
		//Rigidbody rb = GetComponent<Rigidbody>();
		//if (rb != null)
		//{
		//	rb.velocity = Vector3.zero;
		//	rb.angularVelocity = Vector3.zero;
		//}
	}

	public float GetSpeed()
	{
		return m_Agent.velocity.magnitude;
	}

	void MoveTo(Vector3 point)
	{
		//Transform pt = SceneManager.instance.player.transform;
		//float distanceToDestination = Vector3.Distance(point, pt.position);
		//if (distanceToDestination < 1) pt.LookAt(point);

		//m_Agent.stoppingDistance = 1.1f;
		m_Agent.SetDestination(point);
	}

	public void SetTarget(GameObject target)
	{
		//Debug.Log("setting target to [" + target.name + "]");
		SelectedTarget = target;
		float distanceToTarget = Vector3.Distance(SelectedTarget.transform.position, transform.position);
		if (distanceToTarget > 1.2)
		{
			MoveTo(SelectedTarget.transform.position);
		}
		else
		{
			TryToInteract();
		}
	}

	public bool TryToInteract()
	{
		float distanceToTarget = Vector3.Distance(transform.position, new Vector3(SelectedTarget.transform.position.x, 0, SelectedTarget.transform.position.z));
		Interactable ia = SelectedTarget.gameObject.GetComponent<Interactable>();
		if (!ia || distanceToTarget > ia.radius) return false;
		Stop();
		SetDirection();
		transform.LookAt(SelectedTarget.transform.position);
		Enemy e = SelectedTarget.gameObject.GetComponent<Enemy>();
		bool caninteract = (e != null) ? player.Stats.CanAttack() : true;
		bool didinteract = false;
		if (caninteract)
		{
			didinteract = ia.Interact();
			if (e != null && didinteract) DoAttack();
		}
		SelectedTarget = null;
		return didinteract;
	}

	/// <summary>
	/// doesn't move - just turns the player to face that direction
	/// </summary>
	public void SetDirection()
	{
		Ray ray = m_Manager.ActiveCamera.ScreenPointToRay(Input.mousePosition);
		LayerMask movementMask = LayerMask.GetMask("Ground") & ~LayerMask.GetMask("Player");
		if (Physics.Raycast(ray, out RaycastHit hit, movementMask))
		{
			SetDirection(hit.point);
		}
	}

	public void SetDirection(Vector3 target)
	{
		Vector3 direction = (target - transform.position).normalized;
		Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = lookrotation;
	}

	/// <summary>
	/// try to move the player to this location
	/// </summary>
	public void SetDestination()
	{
		Ray ray = m_Manager.ActiveCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		LayerMask movementMask = LayerMask.GetMask("Ground") & ~LayerMask.GetMask("Player");
		if (!Physics.Raycast(ray, out hit, movementMask)) return;
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
		if (hitname.StartsWith("Floor") || hitname.StartsWith("Stairs") || hitname == "Plane" || hitname.EndsWith("Terrain"))// || hitname == "Start" || hitname == "End")
		{
			//Debug.Log("distanceToTarget < ia.radius [" + distanceToTarget + " " + ia.radius + "]");
			//if (SelectedTarget)
			//{
			//	float distanceToTarget = Vector3.Distance(transform.position, new Vector3(SelectedTarget.transform.position.x, 0, SelectedTarget.transform.position.z));
			//	//Debug.Log(name + " SelectedTarget=null");
			//}

			SelectedTarget = null;
			MoveTo(hit.point);
		}
	}

	public void Stop()
	{
		//float distanceToTarget = Vector3.Distance(SelectedTarget.transform.position, SceneManager.instance.player.transform.position);
		//Debug.Log("Stop() distanceToTarget [" + distanceToTarget + "]");
		//m_Agent.enabled = false;
		//m_Agent.velocity = Vector3.zero;
		//m_Agent.enabled = true;
		m_Agent.ResetPath();
	}

	/// <summary>
	/// play the attack animation and sound
	/// </summary>
	public void DoAttack()
	{
		// this should probably time these things. maybe use an animator
		Stop();
		m_Animator.Play("Attack");
		SoundManager.GetCurrent().PlaySound(SoundManager.Sounds.PlayerAttack1, GameManager.GetPlayer().Class);

		Character c = GameManager.GetPlayer();
		if (c.Equipped.righthand != null && c.Equipped.righthand.baseType && c.Equipped.righthand.baseType.GetType() == typeof(WeaponBase))
		{
			c.Equipped.righthand.durability -= 0.1f;
			if (c.Equipped.righthand.durability <= 0)
			{
				c.Equipped.righthand.durability = 0;
				c.InventoryAdd(c.Equipped.righthand);
				c.Equipped.righthand = null;
			}
		}
	}
}
