using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gameplay : MonoBehaviour
{
	//Singleton--------------------------------------------------------------------------------------------------------/
	public static UI_Gameplay instance;
	
	//UI Elements------------------------------------------------------------------------------------------------------/
	//PLAYER
	[SerializeField] private Slider m_FartMeter;
	public float FartMeter
	{
		set
		{
			m_FartMeter.value = value;
		}
	}

	[SerializeField] private Slider m_CloutMeter;
	public float CloutMeter
	{
		set
		{
			m_CloutMeter.value = value;
		}
	}

	[SerializeField] private Text m_NextFart;
	public string NextFart
	{
		set
		{
			m_NextFart.text = value;
		}
	}
	
	//ENVIRONMENT
	[SerializeField] private Slider m_StenchMeter;
	public float StenchMeter
	{
		set
		{
			m_StenchMeter.value = value;
		}
	}

	[SerializeField] private Slider m_NoiseMeter;
	public float NoiseMeter
	{
		set
		{
			m_NoiseMeter.value = value;
		}
	}


	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}

	private void Update()
	{
		
	}
}
