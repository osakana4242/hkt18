﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject target;
	public Vector3 offset = new Vector3(0f, 1f, -5f);
	public Vector3 offset2 = new Vector3(0f, 0f, 1f);
	public float moveSpeed = 20f;
	public float rotSpeed = 20f;
	public Vector3 targetRot;

	public bool useFixedUpdate = true;

	// Start is called before the first frame update
	void Start() {

	}

	void Step() {
		var tpos = target.transform.position;
		var playerRot = target.transform.rotation;
		var offsetRot = Quaternion.Euler(targetRot);

		var cpos = transform.position;
		var crot = transform.rotation;

		crot = Quaternion.Lerp(crot, offsetRot, Time.deltaTime * rotSpeed);

		var tpos2 = tpos + offset; // (trot * offset);
		tpos2 += playerRot * offset2;

		cpos = Vector3.Lerp(cpos, tpos2, Time.deltaTime * moveSpeed);

		transform.rotation = crot;
		transform.position = cpos;
	}

	void FixedUpdate() {
		if (!useFixedUpdate) return;
		Step();
	}

	// Update is called once per frame
	void LateUpdate() {
		if (useFixedUpdate) return;
		Step();
	}
}
