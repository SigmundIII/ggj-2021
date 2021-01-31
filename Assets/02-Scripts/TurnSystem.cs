﻿using System;
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
	private PlayerInteract playerInteract;
	private Follow_Player camera;
	private Ritual_affordance ritual;

	public event Action<int> OnBattlePhaseStart;
	public float fullTime=30;
	private float time = 0;
	public int battleValuePenalty;
	
	private float timer;

	private void Awake() {
		playerInput = FindObjectOfType<PlayerInput>();
		playerInteract = FindObjectOfType<PlayerInteract>();
		storage = FindObjectOfType<Storage>();
		dungeon = FindObjectOfType<CreateDungeon>();
		gameManager = FindObjectOfType<GameManager>();
		camera = FindObjectOfType<Follow_Player>();
		ritual = FindObjectOfType <Ritual_affordance>();
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
				gameManager.characterSheetsManager.Hide();
				StartLootPhase();
				break;
			case TurnPhase.Loot:
				if (currentFloor < maxFloors) {
					camera.ClearList();
					gameManager.DumpLoot();
					storage.NextFloor(currentFloor);
					dungeon.NextFloor(currentFloor);
					FindObjectOfType<PlayerMovement>().ResetFallGuys();
					currentFloor++;
					if (currentFloor < maxFloors) {
						StartPlacePhase();
					}
				}
				else {
					camera.ClearList();
					StartEndingPhase();
				}
				break;
			case TurnPhase.Ending:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void StartEndingPhase() {
		currentPhase = TurnPhase.Ending;
		if (ritual.slider.value < gameManager.maxBattleValue && ritual.slider.value > gameManager.minBattleValue) {
			//Caricamento scena di vittoria
			Debug.Log("You win");
		}
		else {
			//Caricamento scena di sconfitta
			Debug.Log("You lose");
		}
		
	}


	public void StartPlacePhase() {
		playerInput.transform.position = storage.spawnPoint.position;
		currentPhase = TurnPhase.Place;
		playerInput.enabled = true;
	}
	
	public void StartBattlePhase() {
		currentPhase = TurnPhase.Battle;
		playerInput.enabled = false;
		OnBattlePhaseStart?.Invoke(currentFloor);
	}
	
	public void StartLootPhase() {
		currentPhase = TurnPhase.Loot;
		playerInput.enabled = true;
	}
}
