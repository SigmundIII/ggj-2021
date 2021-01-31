using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	private Fade fade;
	

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
		fade = FindObjectOfType<Fade>();

		fade.fadeInComplete += StopFade;
		fade.fadeOutComplete += StopFade;
		fade.fadeOutComplete += StartPlacePhase;
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
				StartLootPhase();
				break;
			case TurnPhase.Loot:
				if (currentFloor < maxFloors) {
					FadeOut();
					camera.ClearList();
					gameManager.DumpLoot();
					storage.NextFloor(currentFloor);
					dungeon.NextFloor(currentFloor);
					FindObjectOfType<PlayerMovement>().ResetFallGuys();
					currentFloor++;
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
		//start new scene
		//playerInput.transform.position = storage.playerSpawn.position;
		if (ritual.slider.value < gameManager.maxBattleValue && ritual.slider.value > gameManager.minBattleValue) {
			//Caricamento scena di vittoria
			HasWon.hasWon = true;
			Debug.Log("You win: "+ritual.slider.value);
		}
		else {
			//Caricamento scena di sconfitta
			HasWon.hasWon = false;
			Debug.Log("You lose with: "+ritual.slider.value);
		}
		SceneManager.LoadScene("BossScene");

	}


	public void StartPlacePhase() {
		if (currentFloor >= maxFloors) {
			return;
		}
		
		playerInput.transform.position = storage.playerSpawn.position;
		currentPhase = TurnPhase.Place;
		playerInput.enabled = true;
		FadeIn();
	}
	
	public void StartBattlePhase() {
		currentPhase = TurnPhase.Battle;
		playerInput.enabled = false;
		OnBattlePhaseStart?.Invoke(currentFloor);
	}

	private void FadeIn() {
		Debug.Log("Fade In");
		fade.FadeIn();
	}

	private void FadeOut() {
		Debug.Log("Fade Out");
		playerInput.enabled = false;
		fade.FadeOut();
	}

	private void StopFade() {
		playerInput.enabled = true;
		Debug.Log("Stop fading");

		if (currentFloor >= maxFloors) {
			camera.ClearList();
			StartEndingPhase();
		}
	}
	
	public void StartLootPhase() {
		currentPhase = TurnPhase.Loot;
		playerInput.enabled = true;
	}
}
