using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnPhase{Place,Battle,Loot}
	

public class TurnSystem : MonoBehaviour {
	[SerializeField] private TurnPhase currentPhase;
	public int maxFloors=3;
	private int currentFloor;

	private PlayerInput playerInput;
	private Storage storage;
	private CreateDungeon dungeon;

	public event Action OnBattlePhaseStart;
	
	private void Awake() {
		playerInput = FindObjectOfType<PlayerInput>();
		storage = FindObjectOfType<Storage>();
		dungeon = FindObjectOfType<CreateDungeon>();
	}

	private void Start() {
		storage.Init(maxFloors);
		dungeon.Init(maxFloors);
		StartPlacePhase();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			StartPlacePhase();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			StartBattlePhase();
		}
		// if (Input.GetKeyDown(KeyCode.Alpha3)) {
		// 	StartLootPhase(); // Changed with N - See GameManager
		// }
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
				if (currentFloor < maxFloors) {
					storage.CloseDoors(currentFloor);
					storage.DestroyFloor(currentFloor);
					currentFloor++;
					StartPlacePhase();
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
		storage.SetSputoPoint(currentFloor);
		storage.OpenDoors(currentFloor);
		currentPhase = TurnPhase.Place;
		playerInput.enabled = true;
	}
	
	public void StartBattlePhase() {
		storage.CloseDoors(currentFloor);
		currentPhase = TurnPhase.Battle;
		playerInput.enabled = false;
		OnBattlePhaseStart?.Invoke();
	}
	
	public void StartLootPhase() {
		storage.OpenDoors(currentFloor);
		currentPhase = TurnPhase.Loot;
		playerInput.enabled = true;
	}
}
