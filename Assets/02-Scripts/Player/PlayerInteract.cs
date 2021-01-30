using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	[SerializeField]private bool canGrab;
	[SerializeField]private bool hasGrabbed;
	[SerializeField]private IGrabbable obj;

	[Header("Interact attributes")] 
	public float throwForce;
	

	public void Grab() {
		if (obj != null) {
			if (canGrab && !hasGrabbed) {
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
		if (grabbable != null && !hasGrabbed) {
			canGrab = true;
			obj = grabbable;
		}
	}
	
	private void OnTriggerExit(Collider other) {
		if (!hasGrabbed) {
			canGrab = false;
			obj = null;
		}
	}
}
