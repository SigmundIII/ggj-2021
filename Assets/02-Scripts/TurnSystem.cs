using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public enum TurnPhase{Place,Battle,Loot,Ending}
	

public class TurnSystem : MonoBehaviour {
	[SerializeField] private TurnPhase currentPhase;
	public int maxFloors=3;
	private int currentFloor;

	private PlayerInput playerInput;
	private Storage storage;
	private CreateDungeon dungeon;
	private GameManager gameManager;

	public event Action OnBattlePhaseStart;
	
	private void Awake() {
		playerInput = FindObjectOfType<PlayerInput>();
		storage = FindObjectOfType<Storage>();
		dungeon = FindObjectOfType<CreateDungeon>();
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Start() {
		storage.Init(maxFloors);
		//+1 perchè c'è la stanza del boss
		dungeon.Init(maxFloors+1);
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
					gameManager.DestroyItems();
					DestroyStorage(currentFloor);
					DestroyDungeon(currentFloor);
					FindObjectOfType<PlayerMovement>().ResetFallGuys();
					currentFloor++;
					if (currentFloor < maxFloors) {
						StartPlacePhase();
					}
				}
				else {
					StartEndingPhase();
				}
				break;
			case TurnPhase.Ending:
				Debug.Log("Niente più piani");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void StartEndingPhase() {
		currentPhase = TurnPhase.Ending;
	}

	public void DestroyStorage(int floor) {
		storage.CloseDoors(floor);
		storage.DestroyFloor(floor);
		storage.DestroyWalls(floor);
		storage.DestroyDoors(floor);
	}

	public void DestroyDungeon(int floor) {
		dungeon.DestroyFloor(floor);
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
