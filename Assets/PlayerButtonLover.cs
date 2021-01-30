using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonLover : MonoBehaviour {
	private bool canPushButton;

	private TurnSystem system;

	private void Awake() {
		system = FindObjectOfType<TurnSystem>();
	}

	public void Push() {
		if (canPushButton) {
			system.NextTurn();
		}
	}
	
	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Button"))
		{
			canPushButton = true;
		}
	}
	
	private void OnTriggerExit(Collider other) {
		if(other.CompareTag("Button"))
		{
			canPushButton = false;
		}
	}
}
