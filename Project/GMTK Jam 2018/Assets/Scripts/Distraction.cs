using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Distraction : MonoBehaviour
{
	public enum DistractionType
	{
		Smell,
		Noise
	}
	public DistractionType distractionType;

	public float distractionAmount = 10.0f;

	public float radius;

	private void Awake()
	{
		radius = GetComponent<CircleCollider2D>().bounds.extents.x;
	}
}
