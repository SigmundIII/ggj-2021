using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnPhase{Place,Battle,Loot}
	

public class TurnSystem : MonoBehaviour {
	[SerializeField] private TurnPhase currentPhase;
	public int currentFloor;

	public List<GameObject> floors=new List<GameObject>();
	
	

	private PlayerInput playerInput;
	private Storage storage;
	
	private void Awake() {
		playerInput = FindObjectOfType<PlayerInput>();
		storage = FindObjectOfType<Storage>();
	}

	private void Start() {
		storage.Init();
		StartPlacePhase();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			StartPlacePhase();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			StartBattlePhase();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			StartLootPhase();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			NextTurn();
		}
	}

	public void NextTurn() {
		switch (currentPhase) {
			case TurnPhase.Place:
				StartBattlePhase();
				break;
			case TurnPhase.Battle:
				StartLootPhase();
				break;
			case TurnPhase.Loot:
				if (currentFloor != floors.Count) {
					Destroy(floors[currentFloor]);
					StartPlacePhase();
					currentFloor++;
				}
				else {
					Debug.Log("Niente più piani");
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
	
	
	public void StartPlacePhase() {
		currentPhase = TurnPhase.Place;
		playerInput.enabled = true;
	}
	
	public void StartBattlePhase() {
		currentPhase = TurnPhase.Battle;
		playerInput.enabled = false;
	}
	
	public void StartLootPhase() {
		currentPhase = TurnPhase.Loot;
		playerInput.enabled = true;
	}
}
