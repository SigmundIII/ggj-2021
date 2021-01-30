using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	[SerializeField]private bool canGrab;
	[SerializeField]private bool hasGrabbed;
	[SerializeField]private List<IGrabbable> obj=new List<IGrabbable>();
	[SerializeField] private GameObject grabVfx;

	[Header("Interact attributes")] 
	public float throwForce;

	private void Awake() {
		grabVfx.SetActive(false);
	}

	public void Grab() {
		if (obj.Count>0) {
			if (canGrab && !hasGrabbed) {
				hasGrabbed = true;
				obj[0].Grabbed(transform);
				grabVfx.SetActive(true);
			}
		}
	}

	public void Release() {
		if (hasGrabbed && obj.Count>0) {
			obj[0].Released();
			hasGrabbed = false;
			grabVfx.SetActive(false);
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
		if (hasGrabbed && obj.Count>0) {
			var force = transform.forward * throwForce;
			hasGrabbed = false;
			obj[0].Throw(force);
			grabVfx.gameObject.SetActive(false);
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
			if (grabbable != null) {
				var Bobj = other.GetComponent<BasicObject>();
				if (Bobj != null) {
					if (!Bobj.grabbed) {
						obj.Remove(grabbable);
					}
				}
				if (obj.Count <= 0) {
					canGrab = false;
				}
			}
	}
}
