using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	//Components & References------------------------------------------------------------------------------------------/
	private Rigidbody2D m_Rig;
	
	//Values
	[SerializeField] private bool m_IsMoving;
	
	//Stats
	[SerializeField] private float m_MoveSpeed = 1;
	[SerializeField] private float m_IdleFartSpeed = 1;
	[SerializeField] private float m_MovingFartSpeed = 1;
	
	[Space]
	
	[SerializeField] private float m_FartMax = 100;
	private float m_FartCurrent;
	public float Fart
	{
		get
		{
			return m_FartCurrent;
		}
		set
		{
			m_FartCurrent = value;
		}
	}

	[Space]
	[SerializeField] private float m_CloutMax = 100;
	private float m_CloutCurrent;
	public float Clout
	{
		get
		{
			return m_CloutCurrent;
		}
		set
		{
			m_CloutCurrent = value;
		}
	}
	

	private void Awake()
	{
		//Assign References
		m_Rig = GetComponent<Rigidbody2D>();
		
		//Set Values
		m_CloutCurrent = m_CloutMax;
	}

	private void Update()
	{
		PlayerInput();
		UpdateUI();
		FartBuildup();
	}

	private void PlayerInput()
	{
		//Movement
		Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		m_Rig.velocity = dir * m_MoveSpeed;
		m_IsMoving = dir.x != 0 || dir.y != 0 ? true : false;
	}

	private void UpdateUI()
	{
		UI_Gameplay.instance.FartMeter = m_FartCurrent / m_FartMax;
		UI_Gameplay.instance.CloutMeter = m_CloutCurrent / m_CloutMax;
	}

	private void FartBuildup()
	{
		float currentSpeed = m_IsMoving ? m_MovingFartSpeed : m_IdleFartSpeed;
		m_FartCurrent += currentSpeed * Time.deltaTime;

		if (m_FartCurrent >= m_FartMax)
		{
			OnFart();
		}
		
	}

	private void OnFart()
	{
		m_FartCurrent = 0.0f;
		
		Debug.Log("FART");
	}
}
