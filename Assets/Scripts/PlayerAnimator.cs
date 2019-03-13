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
				m_Animator.Play("Attack");
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
	}

	void GetInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Debug.Log("shift attack");
				//m_Animator.SetTrigger("Attack");
				m_Animator.Play("Attack");
				return;
			}
			SetDestination();
		}
		else if (Input.GetMouseButton(0))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Debug.Log("shift attack went to wrong check?");
				//m_Animator.SetTrigger("Attack");
				//m_Animator.Play("Attack");
				return;
			}
			SetDestination();
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (++m_Manager.Settings.CameraZoom > 2) m_Manager.Settings.CameraZoom = 0;
		}
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		// fucking idiots. sign of zero should return zero, not one
		//Debug.Log(string.Format("{0} {1}", scroll, Mathf.Sign(scroll)));
		//float sign = Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));
		//m_Manager.Settings.CameraZoom = Mathf.Clamp(m_Manager.Settings.CameraZoom + Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")), 0, 2);
		if (scroll > 0)
		{
			//if (++m_Manager.Settings.CameraZoom > 2) m_Manager.Settings.CameraZoom = 2;
			m_Manager.Settings.CameraZoom++;
		}
		else if (scroll < 0)
		{
			//if (--m_Manager.Settings.CameraZoom < 0) m_Manager.Settings.CameraZoom = 0;
			m_Manager.Settings.CameraZoom--;
		}
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
			if (hitname.StartsWith("Floor") || hitname.StartsWith("Stairs") || hitname == "Start" || hitname == "End")
			{
				m_Manager.SelectedTarget = null;
				MoveTo(hit.point);
			}
		}
	}
}
