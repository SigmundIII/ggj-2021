using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	private Rigidbody rb;
	public GameObject model;

	[Header("MovementStats")] 
	public float speed;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 direction) {
		Vector3 destination = transform.position + (direction * speed);
		model.transform.LookAt(destination);
		rb.MovePosition(destination);
	}

	public void SetOrientation(Camera camera) {
		var rotationY = camera.transform.eulerAngles.y;
		transform.eulerAngles=new Vector3(0,rotationY,0);
	}

	public void FallGuys() {
		rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
	}

	public void NotFallGuys() {
		//Non si freeza la rotazione
		rb.constraints = RigidbodyConstraints.FreezeRotation |  RigidbodyConstraints.FreezePositionY;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Floor")) {
			NotFallGuys();
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.CompareTag("Floor")) {
			FallGuys();
		}
	}

}
