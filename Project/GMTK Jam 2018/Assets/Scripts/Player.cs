using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	//Components & References------------------------------------------------------------------------------------------/
	private Rigidbody2D m_Rig;
	
	//Values-----------------------------------------------------------------------------------------------------------/
	[SerializeField] private bool m_IsMoving;
	
	
	//Stats------------------------------------------------------------------------------------------------------------/
	//Player
	[SerializeField] private float m_MoveSpeed = 1;
	[SerializeField] private float m_IdleFartSpeed = 1;
	[SerializeField] private float m_MovingFartSpeed = 1;
	[SerializeField] private float m_FartMax = 100;
	[SerializeField] private float m_CloutMax = 100;
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
	
	//Environment
	[SerializeField] private float m_StenchMax = 100;
	private float m_StinkCurrent;
	[SerializeField] private float m_NoiseMax = 100;
	private float m_NoiseCurrent;
	
	//Farts
	private enum FartSmell
	{
		Unsmellable,
		Smelly,
		Putrid
	}
	private FartSmell m_FartSmell;

	private enum FartNoise
	{
		Silent,
		Noticable,
		Loud
	}
	private FartNoise m_FartNoise;

	private enum FartWetness
	{
		Dry,
		Moist,
		Wet
	}
	private FartWetness m_FartWetness;
	
	private void Awake()
	{
		//Assign References
		m_Rig = GetComponent<Rigidbody2D>();
		
		//Set Values
		m_CloutCurrent = m_CloutMax;
	}

	private void Start()
	{
		NextFart();
	}

	private void Update()
	{
		PlayerInput();
		UpdateUI();
		FartBuildup();
		EnvironmentDetection();
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
		UI_Gameplay.instance.StenchMeter = m_StinkCurrent / m_StenchMax;
		UI_Gameplay.instance.NoiseMeter = m_NoiseCurrent / m_NoiseMax;
	}

	private void FartBuildup()
	{
		float currentSpeed = m_IsMoving ? m_MovingFartSpeed : m_IdleFartSpeed;
		m_FartCurrent += currentSpeed * Time.deltaTime;

		if (m_FartCurrent >= m_FartMax)
		{
			StartCoroutine(OnFart());
		}
		
	}

	private IEnumerator OnFart()
	{
		GameObject obj = Instantiate(Resources.Load("Hazard") as GameObject, transform.position, Quaternion.identity);
		Hazard hazard = obj.GetComponent<Hazard>();
		
		
		float f = Fart;
		float timer = 0.5f;
		float t = 0.0f;
		while (t < timer)
		{
			t += Time.deltaTime;

			Fart = Mathf.Lerp(f, 0, t / timer);
			
			yield return null;
		}
		
		NextFart();
	}

	private void NextFart()
	{
		m_FartSmell = (FartSmell) Random.Range(0, System.Enum.GetValues(typeof(FartSmell)).Length);
		m_FartNoise = (FartNoise) Random.Range(0, System.Enum.GetValues(typeof(FartNoise)).Length);
		m_FartWetness = (FartWetness) Random.Range(0, System.Enum.GetValues(typeof(FartWetness)).Length);

		UI_Gameplay.instance.NextFart = "A " + m_FartSmell + ", " + m_FartNoise + ", but " + m_FartWetness + " fart.";
	}

	private void EnvironmentDetection()
	{
		float stench = 0;
		float noise = 0;
		
		RaycastHit2D[] env = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.up, 1.0f, 1 << LayerMask.NameToLayer("Environment"));
		foreach (RaycastHit2D obj in env)
		{
			Distraction distraction = obj.transform.GetComponent<Distraction>();
			if (distraction != null)
			{
				float dist = Vector2.Distance(transform.position, distraction.transform.position);
				float falloff = distraction.falloff.Evaluate(dist / distraction.radius );
				float output = distraction.distractionAmount * falloff;
				
				switch (distraction.distractionType)
				{
						case Distraction.DistractionType.Smell:
							stench += output;
							break;
						
						case Distraction.DistractionType.Noise:
							noise += output;
							break;
				}
			}
		}

		m_StinkCurrent = stench;
		m_NoiseCurrent = noise;
	}
}
