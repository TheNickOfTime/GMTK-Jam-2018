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
			m_FartMeter.transform.Find("Text").GetComponent<Text>().text = (value * 100).ToString("N0") + "%";
		}
	}

	[SerializeField] private Slider m_CloutMeter;
	public float CloutMeter
	{
		set
		{
			m_CloutMeter.value = value;
			m_CloutMeter.transform.Find("Text").GetComponent<Text>().text = (value * 100).ToString("N0") + "%";
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
			m_StenchMeter.transform.Find("Text").GetComponent<Text>().text = (value * 100).ToString("N0") + "%";
		}
	}

	[SerializeField] private Slider m_NoiseMeter;
	public float NoiseMeter
	{
		set
		{
			m_NoiseMeter.value = value;
			m_NoiseMeter.transform.Find("Text").GetComponent<Text>().text = (value * 100).ToString("N0") + "%";
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
