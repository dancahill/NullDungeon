using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
	GameManager m_Manager;
	NavMeshAgent m_Agent;
	Animator m_Animator;

	void Start()
	{
		//m_Manager = Resources.FindObjectsOfTypeAll<Test1Manager>()[0];
		m_Manager = GameManager.instance;
		//m_Agent = GetComponent<NavMeshAgent>();
		m_Agent = gameObject.AddComponent<NavMeshAgent>();
		m_Agent.speed = 3.0f;
		m_Agent.angularSpeed = 500;
	}

	void Update()
	{
		const float locomotionAnimationSmoothTime = .1f;
		float speedPercent = m_Agent.velocity.magnitude / m_Agent.speed;

		// bad code - lets us change avatars at runtime
		m_Animator = GetComponentInChildren<Animator>();
		if (m_Animator)
		{
			m_Animator.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
		}
		GetInput();
		if (m_Manager.SelectedTarget != null)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, m_Manager.SelectedTarget.transform.position);
			if (distanceToEnemy < 1.1)
			{
				Debug.Log("close! attack?!?");
				//transform.LookAt(m_Manager.SelectedTarget.transform);
				//m_Animator.SetTrigger("Attack");
				DoAttack();
				Enemy e = m_Manager.SelectedTarget.gameObject.GetComponent<Enemy>();
				e.TakeDamage(50);
				m_Manager.SelectedTarget = null;
			}
			else
			{
				//Debug.Log("trying to follow " + m_Manager.SelectedTarget.name);
				MoveTo(m_Manager.SelectedTarget.transform.position);
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

	void GetInput()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetMouseButtonDown(0))
			{
				SetDirection();
				DoAttack();
			}
		}
		else
		{
			if (Input.GetMouseButton(0))
			{
				SetDestination();
			}
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (++m_Manager.Settings.CameraZoom > 2) m_Manager.Settings.CameraZoom = 0;
		}
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		// fucking idiots. sign of zero should return zero, not one
		if (scroll != 0) m_Manager.Settings.CameraZoom += Mathf.Sign(scroll);
		m_Manager.Settings.CameraZoom = Mathf.Clamp(m_Manager.Settings.CameraZoom, 0, 2);
	}

	void MoveTo(Vector3 point)
	{
		m_Agent.SetDestination(point);
	}

	public void SetTarget(GameObject target)
	{
		m_Manager.SelectedTarget = target;
		float distanceToEnemy = Vector3.Distance(m_Manager.SelectedTarget.transform.position, m_Manager.player.transform.position);
		Debug.Log("clicked on '" + target.name + "' distance = " + distanceToEnemy.ToString("0.0"));
		MoveTo(m_Manager.SelectedTarget.transform.position);
	}

	/// <summary>
	/// doesn't move - just turns the player to face that direction
	/// </summary>
	void SetDirection()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask movementMask = LayerMask.GetMask("Ground");
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, movementMask))
		{
			// looks ugly turning this suddenly, but good enough for now
			transform.LookAt(hit.point);
		}
	}

	/// <summary>
	/// try to move the player to this location
	/// </summary>
	void SetDestination()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask movementMask = LayerMask.GetMask("Ground");
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, movementMask))
		{
			string hitname = hit.transform.name;
			//if (hit.transform.name.StartsWith("Floor"))
			//if (hit.transform.gameObject.layer == movementMask)
			//if ((movementMask & 1 << hit.transform.gameObject.layer) == 1 << hit.transform.gameObject.layer)
			// this is ugly - find a better solution
			if (hitname.StartsWith("Floor") || hitname.StartsWith("Stairs") || hitname == "Plane" || hitname == "Terrain" || hitname == "Start" || hitname == "End")
			{
				m_Manager.SelectedTarget = null;
				MoveTo(hit.point);
			}
		}
	}

	/// <summary>
	/// play the attack animation and sound
	/// </summary>
	void DoAttack()
	{
		// this should probably time these things. maybe use an animator
		m_Animator.Play("Attack");
		m_Manager.m_SoundManager.PlaySound("File00000063");//64 is an alternate
	}
}
