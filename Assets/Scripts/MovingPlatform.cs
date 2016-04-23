﻿using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public Vector3 PositionA = new Vector3(0, 1);
	public Vector3 PositionB = new Vector3(0, -1);
	public float MoveSpeed = 5f;
	public bool Active = true;
	public bool StartMovingToA = true;

	private bool movingToA;

	private float startTime;
	private float journeyLength;

	void Start() {
		startTime = Time.time;
		movingToA = StartMovingToA;
	}
	void FixedUpdate() {
		Vector3 beforePos = transform.position;
		float distCovered = (Time.time - startTime) * MoveSpeed;

		if (movingToA) {
			journeyLength = Vector3.Distance(PositionA, PositionB);
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(PositionA, PositionB, fracJourney);
			if (Mathf.Abs(journeyLength - distCovered) < .05f) {
				startTime = Time.time;
				movingToA = false;
			}
		} else {
			journeyLength = Vector3.Distance(PositionB, PositionA);
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(PositionB, PositionA, fracJourney);
			if (Mathf.Abs(journeyLength - distCovered) < .05f) {
				startTime = Time.time;
				movingToA = true;
			}
		}


	}
}