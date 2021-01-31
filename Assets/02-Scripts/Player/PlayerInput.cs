using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;
    private PlayerButtonLover playerButton;
    public AudioSource audioSource;
    private Camera camera;

    
    public AudioEvent ghostInteract;
    public AudioEvent ghostMove;
    
    public LayerMask itemsLayer;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        playerButton = GetComponentInChildren<PlayerButtonLover>();
        camera = Camera.main;
    }

    private void Update() {
        if (!audioSource.isPlaying) {
            ghostMove.Play(audioSource);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            playerInteract.GrabOrRealease();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            ghostInteract.Play(audioSource);
            playerInteract.Throw();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            playerButton.Push();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, itemsLayer)) {
            var item = hit.transform.gameObject.GetComponent<Item>();
            if (item != null) {
                ItemPopup.Show(item);
            }
        } else {
            ItemPopup.Hide();
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
