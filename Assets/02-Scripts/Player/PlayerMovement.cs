using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	private Rigidbody rb;

	[Header("MovementStats")] 
	public float speed;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 direction) {
		Vector3 destination = transform.position + (direction * speed);
		transform.LookAt(destination);
		rb.MovePosition(destination);
	}

	public void FallGuys() {
		rb.isKinematic = false;
	}

	public void NotFallGuys() {
		rb.isKinematic = true;
	}


}
