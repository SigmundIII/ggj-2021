﻿using DefaultNamespace;
using UnityEngine;

public class BasicObject : MonoBehaviour,IGrabbable {
	private Rigidbody rb;
	private Storage storage;
	private GameManager gameManager;
	private Item item;

	public bool grabbed;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		storage = FindObjectOfType<Storage>();
		gameManager = FindObjectOfType<GameManager>();
		item = GetComponent<Item>();
	}

	private void Update() {
		if (!grabbed) {
			if (transform.position.x > storage.treasonPoint.position.x) {
				// Inside Storage
				transform.SetParent(storage.transform);
				if (!storage.items.Contains(item)) {
					if (item != null) {
						gameManager.items.Remove(item);
						storage.AddItem(item);
					}
				}
			} else {
				// Outside Storage
				transform.SetParent(gameManager.transform);
				if (!gameManager.items.Contains(item)) {
					if (item != null) {
						gameManager.items.Add(item);
						storage.items.Remove(item);
					}
				}
			}
		}
	}

	public void Grabbed(Transform parent) {
		transform.SetParent(parent);
		gameManager.items.Remove(item);
		storage.items.Remove(item);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		rb.useGravity = false;
		rb.isKinematic = true;
		grabbed = true;
	}

	public void Released() {
		transform.SetParent(null);
		rb.useGravity = true;
		rb.isKinematic = false;
		grabbed = false;
	}

	public void Throw(Vector3 force) {
		transform.SetParent(null);
		rb.useGravity = true;
		rb.isKinematic = false;
		rb.AddForce(force);
		grabbed = false;
	}

	public void Stop() {
		rb.velocity=Vector3.zero;
	}
}
