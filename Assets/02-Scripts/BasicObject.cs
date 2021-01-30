using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObject : MonoBehaviour,IGrabbable {
	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	public void Grabbed(Transform parent) {
		Vector3 scale = transform.localScale;
		transform.parent = parent;
		transform.position = parent.position;
		transform.rotation = Quaternion.identity;
		transform.localScale = scale;
		rb.useGravity = false;
		rb.isKinematic = true;
	}

	public void Released() {
		transform.parent = null;
		rb.useGravity = true;
		rb.isKinematic = false;
	}

	public void Throw(Vector3 force) {
		transform.parent = null;
		rb.useGravity = true;
		rb.isKinematic = false;
		rb.AddForce(force);
	}

	private void OnBecameInvisible() {
		Destroy(this.gameObject);
	}

	public void Stop() {
		rb.velocity=Vector3.zero;
	}
}
