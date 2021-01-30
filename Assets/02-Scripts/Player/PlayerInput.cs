using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;
    private PlayerButtonLover playerButton;
    private Camera camera;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        playerButton = GetComponentInChildren<PlayerButtonLover>();
        camera = Camera.main;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            playerInteract.GrabOrRealease();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            playerInteract.Throw();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            playerButton.Push();
        }
        
    }

    private void FixedUpdate() {
        playerMovement.SetOrientation(camera);
        playerMovement.Move(GetDirection());
    }

    private Vector3 GetDirection() {
        Vector3 dir=Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            dir += transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            dir -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D)) {
            dir += transform.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            dir -= transform.right;
        }
        dir.Normalize();
        return dir;
    }
}
