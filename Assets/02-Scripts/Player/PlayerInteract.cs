using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	[SerializeField]private bool canGrab;
	[SerializeField]private bool hasGrabbed;
	[SerializeField]private List<IGrabbable> obj=new List<IGrabbable>();

	[Header("Interact attributes")] 
	public float throwForce;
	

	public void Grab() {
		if (obj[0] != null) {
			if (canGrab && !hasGrabbed) {
				hasGrabbed = true;
				obj[0].Grabbed(transform);
			}
		}
	}

	public void Release() {
		if (hasGrabbed && obj[0]!=null) {
			obj[0].Released();
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
			hasGrabbed = false;
			obj[0].Throw(force);
		}
	}
	
	private void OnTriggerEnter(Collider other) {
		var grabbable=other.GetComponent<IGrabbable>();
		if (grabbable != null && !hasGrabbed) {
			canGrab = true;
			obj.Add(grabbable);
		}
	}
	
	private void OnTriggerExit(Collider other) {
			var grabbable=other.GetComponent<IGrabbable>();
			if (grabbable != null && !hasGrabbed) {
				obj.Remove(grabbable);
				if (obj.Count <= 0) {
					canGrab = false;
				}
			}
	}
}
