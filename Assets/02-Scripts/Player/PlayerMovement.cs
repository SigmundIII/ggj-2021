using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	private Rigidbody rb;
	public GameObject model;

	[Header("MovementStats")] 
	public float speed;

	private List<Collider> floorColliders = new List<Collider>();

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 direction) {
		Vector3 destination = transform.position + direction.normalized * (speed * Time.deltaTime);
		model.transform.LookAt(destination);
		Vector3 planarVel = direction * (speed * Time.deltaTime);
		rb.velocity = new Vector3(planarVel.x, rb.velocity.y, planarVel.z);
		// rb.MovePosition(destination);
	}

	public void SetOrientation(Camera camera) {
		var rotationY = camera.transform.eulerAngles.y;
		transform.eulerAngles=new Vector3(0,rotationY,0);
	}

	public void ResetFallGuys() {
		floorColliders.Clear();
		FallGuys();
	}

	public void FallGuys() {
		if (floorColliders.Count <= 0) {
			rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
		}
	}

	public void NotFallGuys() {
		rb.constraints = RigidbodyConstraints.FreezeRotation |  RigidbodyConstraints.FreezePositionY;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Floor")) {
			floorColliders.Add(other.collider);
			NotFallGuys();
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.CompareTag("Floor")) {
			floorColliders.Remove(other.collider);
			FallGuys();
		}
	}

}
