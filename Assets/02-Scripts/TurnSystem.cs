using System;
using DefaultNamespace;
using UnityEngine;

public enum TurnPhase{Place,Battle,Loot,Ending}
	

public class TurnSystem : MonoBehaviour {
	public TurnPhase currentPhase;
	public int maxFloors=3;
	private int currentFloor;

	private PlayerInput playerInput;
	private Storage storage;
	private CreateDungeon dungeon;
	private GameManager gameManager;

	public event Action<int> OnBattlePhaseStart;
	public float fullTime=30;
	private float time = 0;
	public int battleValuePenalty;
	
	
	
	private float timer;

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
		SetTimer();
		StartPlacePhase();
	}

	public void SetTimer() {
		int sum = 0;
		foreach (Character hero in gameManager.heroes) {
			sum += hero.BattleValue;
		}
		time = fullTime-(sum / battleValuePenalty);
		timer = 0;
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
		
		switch (currentPhase) {
			case TurnPhase.Place:
				timer += Time.deltaTime;
				if (timer >= time) {
					NextTurn();
					SetTimer();
				}
				break;
		}
	}

	public void NextTurn() {
		switch (currentPhase) {
			case TurnPhase.Place:
				StartBattlePhase();
				break;
			case TurnPhase.Battle:
				gameManager.aftermath.Hide();
				StartLootPhase();
				break;
			case TurnPhase.Loot:
				if (currentFloor < maxFloors) {
					gameManager.DumpLoot();
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
		OnBattlePhaseStart?.Invoke(currentFloor);
	}
	
	public void StartLootPhase() {
		storage.OpenDoors(currentFloor);
		currentPhase = TurnPhase.Loot;
		playerInput.enabled = true;
	}
}
