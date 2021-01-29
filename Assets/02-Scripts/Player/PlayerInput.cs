using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;
    private Camera camera;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        camera = Camera.main;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            playerInteract.GrabOrRealease();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            playerInteract.Throw();
        }
    }

    private void FixedUpdate() {
        playerMovement.Move(GetDirection());
    }

    private Vector3 GetDirection() {
        Vector3 dir=Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            dir += camera.transform.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            dir -= camera.transform.up;
        }
        if (Input.GetKey(KeyCode.D)) {
            dir += camera.transform.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            dir -= camera.transform.right;
        }
        dir.Normalize();
        return dir;
    }
}
