using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
	private const float decayRate = 2;
	
	public float smell;
	public float noise;

	public float radius;

	private void Awake()
	{
		radius = GetComponent<CircleCollider2D>().bounds.extents.x;
	}

	private void Update()
	{
		smell -= decayRate * Time.deltaTime;
		smell = Mathf.Clamp(smell, 0, 100);

		noise -= decayRate * Time.deltaTime;
		noise = Mathf.Clamp(noise, 0, 100);

		bool destroy = smell <= 0 && noise <= 0 ? true : false;
		if (destroy)
		{
			Destroy(gameObject);
		}
	}
}
