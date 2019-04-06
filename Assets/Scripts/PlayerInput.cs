using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
	GameManager m_Manager;
	Player player;

	private void Start()
	{
		m_Manager = GameManager.instance;
		player = gameObject.GetComponentInParent<Player>();
	}

	private void Update()
	{
		GetInput();
	}

	void GetInput()
	{
		bool uitouched = false;

		// Check if there is a touch
		// NONE OF THIS FUCKING WORKS ON MOBILE
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			// Check if finger is over a UI element
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
			{
				uitouched = true;
				//Debug.Log("Touched the UI");
			}
		}
		if (EventSystem.current.IsPointerOverGameObject())
		{
			uitouched = true;
		}
		if (!uitouched)
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (player.Stats.CanAttack())
					{
						player.Animator.Stop();
						player.Animator.SetDirection();
						player.Animator.DoAttack();
						int damage;
						player.Stats.CalculateDamage(null, out damage);
					}
				}
			}
			else
			{
				if (Input.GetMouseButton(0))
				{
					player.Animator.SetDestination();
				}
			}
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			// fucking idiots. sign of zero should return zero, not one
			if (scroll != 0) m_Manager.Settings.CameraZoom += Mathf.Sign(scroll);
			m_Manager.Settings.CameraZoom = Mathf.Clamp(m_Manager.Settings.CameraZoom, 0, 2);
		}
		else
		{
			//Debug.Log("pointing at the UI");
		}

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			if (Input.GetKeyDown(KeyCode.M))
			{
				GameManager.instance.Settings.PlayMusic = !GameManager.instance.Settings.PlayMusic;
				GameSave.SaveSettings();
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				GameManager.instance.Settings.PlaySound = !GameManager.instance.Settings.PlaySound;
				GameSave.SaveSettings();
			}
		}
		m_Manager.Settings.ShowItemsOnGround = Input.GetKey(KeyCode.LeftAlt);
		if (Input.GetKeyDown(KeyCode.C))
		{
			GameCanvas c = FindObjectOfType<GameCanvas>();
			c.OpenCharacterPanel(!c.IsCharacterPanelOpen());

		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			GameCanvas c = FindObjectOfType<GameCanvas>();
			c.OpenInventoryPanel(!c.IsInventoryPanelOpen());
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameCanvas c = FindObjectOfType<GameCanvas>();
			c.OpenCharacterPanel(false);
			c.OpenInventoryPanel(false);
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (++m_Manager.Settings.CameraZoom > 2) m_Manager.Settings.CameraZoom = 0;
		}
	}
}
