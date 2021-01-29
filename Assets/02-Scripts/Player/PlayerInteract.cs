using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	private bool canGrab;
	private bool hasGrabbed;
	private IGrabbable obj;

	[Header("Interact attributes")] 
	public float throwForce;
	

	public void Grab() {
		if (obj != null) {
			if (canGrab) {
				hasGrabbed = true;
				obj.Grabbed(transform);
			}
		}
	}

	public void Release() {
		if (hasGrabbed && obj!=null) {
			obj.Released();
			hasGrabbed = false;
		}
	}

	public void GrabOrRealease() {
		if (hasGrabbed) {
			Release();
		}
		else {
			Grab();
		}
	}

	public void Throw() {
		if (hasGrabbed) {
			var force = transform.forward * throwForce;
			obj.Released();
			hasGrabbed = false;
			obj.Throw(force);
		}
	}
	
	private void OnTriggerEnter(Collider other) {
		var grabbable=other.GetComponent<IGrabbable>();
		if (grabbable != null) {
			canGrab = true;
			obj = grabbable;
		}
	}
	
	private void OnTriggerExit(Collider other) {
		if (!hasGrabbed) {
			canGrab = true;
			obj = null;
		}
	}
}
