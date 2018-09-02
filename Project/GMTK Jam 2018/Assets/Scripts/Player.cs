using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
	//Components & References------------------------------------------------------------------------------------------/
	private Rigidbody2D m_Rig;
	private Animator m_Anim;
	
	//Values-----------------------------------------------------------------------------------------------------------/
	private bool m_IsMoving;

    [SerializeField] private GameObject gameOverMenu;
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
			m_FartCurrent = Mathf.Clamp(value, 0, 100);
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
			m_CloutCurrent = Mathf.Clamp(value, 0, 100);
		}
	}
	
	//Environment
	[SerializeField] private float m_StenchMax = 100;
	private float m_StinkCurrent;
	public float Stink
	{
		set
		{
			m_StinkCurrent = Mathf.Clamp(value, 0, 100);

		}
	}
	
	[SerializeField] private float m_NoiseMax = 100;
	private float m_NoiseCurrent;
	public float Noise
	{
		set
		{
			m_NoiseCurrent = Mathf.Clamp(value, 0, 100);
		}
	}
	
	//Farts
	public AnimationCurve m_Falloff = AnimationCurve.EaseInOut(0, 1, 1, 0);
	
	private enum FartSmell
	{
		Odorless,
		Smelly,
		Putrid
	}
	private FartSmell m_FartSmell;

	private enum FartNoise
	{
		Silent,
		Audible,
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
	
	//Audio------------------------------------------------------------------------------------------------------------/
	[SerializeField] private AudioClip m_LoudFart;
	[SerializeField] private AudioClip m_MediumFart;
	[SerializeField] private AudioClip m_SoftFart;
	
	private void Awake()
	{
		//Assign References
		m_Rig = GetComponent<Rigidbody2D>();
		m_Anim = GetComponent<Animator>();
		
		//Set Values
		m_CloutCurrent = m_CloutMax;
	}

	private void Start()
	{
		NextFart();
        Time.timeScale = 1.0f;
	}

	private void Update()
	{
		PlayerInput();
		UpdateUI();
		FartBuildup();
		EnvironmentDetection();
        GameOverScreen();

		m_Anim.SetBool("IsWalking", m_IsMoving);
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
		SpawnHazard();
		DetermineClout();

		switch (m_FartNoise)
		{
			case FartNoise.Silent:
				AudioSource.PlayClipAtPoint(m_SoftFart, transform.position);
				break;
			case FartNoise.Audible:
				AudioSource.PlayClipAtPoint(m_MediumFart, transform.position);
				break;
			case FartNoise.Loud:
				AudioSource.PlayClipAtPoint(m_LoudFart, transform.position);
				break;
		}
		
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

	private void DetermineClout()
	{
		float stinkDeficit = m_StinkCurrent;
		switch (m_FartSmell)
		{
			case FartSmell.Odorless:
				stinkDeficit -= 5;
				break;
			case FartSmell.Smelly:
				stinkDeficit -= 35;
				break;
			case FartSmell.Putrid:
				stinkDeficit -= 70;
				break;
		}
		
		
		float noiseDeficit = m_NoiseCurrent;
		switch (m_FartNoise)
		{
			case FartNoise.Silent:
				noiseDeficit -= 5;
				break;
			case FartNoise.Audible:
				noiseDeficit -= 35;
				break;
			case FartNoise.Loud:
				noiseDeficit -= 70;
				break;
		}

		float loss = stinkDeficit + noiseDeficit;
		loss *= loss > 0 ? 0.2f : 1;

		Clout += loss;
	}

	private void SpawnHazard()
	{
		GameObject obj = Instantiate(Resources.Load("Hazard") as GameObject, transform.position, Quaternion.identity);
		Hazard hazard = obj.GetComponent<Hazard>();

		switch (m_FartSmell)
		{
			case FartSmell.Odorless:
				hazard.smell = 5;
				break;
			case FartSmell.Smelly:
				hazard.smell = 35;
				break;
			case FartSmell.Putrid:
				hazard.smell = 70;
				break;
		}

		switch (m_FartNoise)
		{
			case FartNoise.Silent:
				hazard.noise = 5;
				break;
			case FartNoise.Audible:
				hazard.noise = 35;
				break;
			case FartNoise.Loud:
				hazard.noise = 70;
				break;
		}
	}

	private void NextFart()
	{
		m_FartSmell = (FartSmell) Random.Range(0, System.Enum.GetValues(typeof(FartSmell)).Length);
		m_FartNoise = (FartNoise) Random.Range(0, System.Enum.GetValues(typeof(FartNoise)).Length);
		m_FartWetness = (FartWetness) Random.Range(0, System.Enum.GetValues(typeof(FartWetness)).Length);

		UI_Gameplay.instance.NextFart = "A " + m_FartSmell + ", but " + m_FartNoise + /*", but " + m_FartWetness +*/ " fart.";
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
				float falloff = m_Falloff.Evaluate(dist / distraction.radius );
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
			else
			{
				Hazard hazard = obj.transform.GetComponent<Hazard>();
				if (hazard != null)
				{
					float dist = Vector2.Distance(transform.position, hazard.transform.position);
					float falloff = m_Falloff.Evaluate(dist / hazard.radius );
					
					stench -= hazard.smell * falloff;
					noise -= hazard.noise * falloff;
				}
			}
		}

		m_StinkCurrent = stench;
		m_NoiseCurrent = noise;
	}

    public void GameOverScreen()
    {
        if (m_CloutCurrent <= 0)
        {
            Time.timeScale = 0.0f;
            gameOverMenu.SetActive(true);
        }
        else
        {
            gameOverMenu.SetActive(false);
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
