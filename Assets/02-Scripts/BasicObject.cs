using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BasicObject : MonoBehaviour,IGrabbable {
	private Rigidbody rb;
	private Storage storage;
	private GameManager gameManager;

	public bool grabbed;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		storage = FindObjectOfType<Storage>();
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Update() {
		if (!grabbed) {
			if (transform.position.x > storage.treasonPoint.position.x) {
				// Inside Storage
				transform.SetParent(storage.transform);
			} else {
				// Outside Storage
				transform.SetParent(gameManager.transform);
			}
		}
	}

	public void Grabbed(Transform parent) {
		Vector3 scale = transform.localScale;
		transform.parent = parent;
		transform.position = parent.position;
		transform.rotation = Quaternion.identity;
		transform.localScale = scale;
		transform.eulerAngles = parent.eulerAngles+new Vector3(90, 0, 90);
		rb.useGravity = false;
		rb.isKinematic = true;
		grabbed = true;
	}

	public void Released() {
		transform.parent = null;
		rb.useGravity = true;
		rb.isKinematic = false;
		grabbed = false;
	}

	public void Throw(Vector3 force) {
		transform.parent = null;
		rb.useGravity = true;
		rb.isKinematic = false;
		rb.AddForce(force);
		grabbed = false;
	}

	public void Stop() {
		rb.velocity=Vector3.zero;
	}
}
