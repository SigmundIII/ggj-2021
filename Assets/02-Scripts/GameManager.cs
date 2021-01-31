using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Visual_Log;

namespace DefaultNamespace {
	[System.Serializable]
	public class FloorEnemy {
		public List<CharacterClass> enemies;
	}
	
	public class GameManager : MonoBehaviour {

		public int MAXSLIDERVALUE = 0;
		public int minBattleValue;
		public int maxBattleValue;
		[SerializeField] private CharacterClass[] party;
		public Button button;
		
		[SerializeField] private List<FloorEnemy> enemyParty=new List<FloorEnemy>();
		[Space]
		public UICharacterSheetsManager characterSheetsManager;
		public float timescale;
		
		[HideInInspector]
		public Character[] heroes;
		private Character[] enemies;
		
		private Battle battle;

		private TurnSystem turnSystem;
		
		private bool _battleEnded;

		private Dump dump;
		
		public List<Item> items;

		private Ritual_affordance _ritual;

		public bool battleEnded;

		private void Awake() {
			EventSystem.current.sendNavigationEvents = false;
			
			//_ritual = FindObjectOfType<Ritual_affordance>();
			turnSystem = FindObjectOfType<TurnSystem>();
			turnSystem.OnBattlePhaseStart += StartBattle;

			dump = FindObjectOfType<Dump>();
			
			heroes = new Character[party.Length];
			for (int i = 0; i < heroes.Length; i++) {
				heroes[i] = new Character(party[i]);
			}
			_ritual = FindObjectOfType<Ritual_affordance>();
			GetEnemies(0);
			
		}

		public void GetEnemies(int currentFloor) {
			enemies = new Character[enemyParty[currentFloor].enemies.Count];
			for (int i = 0; i < enemies.Length; i++) {
				enemies[i] = new Character(enemyParty[currentFloor].enemies[i]);
				enemies[i].name += $" {i}";
			}
		}

		private void Start() {
			VisualLog.Hide();
		}


		private void Update() {
			if (_battleEnded && Input.GetKeyDown(KeyCode.Space)) {
				button.interactable = true;
				VisualLog.Hide();
				AssignLoot();
				characterSheetsManager.Show();
				battleEnded = true;
				_battleEnded = false;
				_ritual.StartCoroutine(_ritual.Ritual_progression());
				FindObjectOfType<PlayerInput>().enabled = true;
				turnSystem.NextTurn();
			}
		}

		public void DumpLoot() {
			for (int i = 0; i < items.Count; i++) {
				dump.AddGarbage(items[i].transform);
				items.RemoveAt(i);
			}
		}

		private void StartBattle(int currentfloor) {
			button.interactable = false;
			_battleEnded = false;
			battleEnded = false;
			Time.timeScale = timescale;
			GetEnemies(currentfloor);
			battle = new Battle(heroes, enemies);
			characterSheetsManager.Hide();
			VisualLog.Show();
			StartCoroutine(battle.BattleCoroutine(EndBattle));
		}

		private void EndBattle() {
			_battleEnded = true;
			Time.timeScale = 1;
		}

		private void AssignLoot() {
			// string log = "";
			// foreach (Item item in loot) {
			// 	log += $"{item.Name} - {item.BattleValue}\n";
			// }
			// Debug.Log(log);
			var _heroes = new List<Character>(heroes);
			_heroes.Sort(Utility.SortCharacters);
			_heroes.Reverse();
			for (int i = 0; i < items.Count; i++) {
				Item item = items[i];
				foreach (Character hero in _heroes) {
					if (hero.CanEquip(item) && hero.Equip(item)) {
						// Debug.Log($"{hero.name} equipped {item.Name}");
						items[i].gameObject.SetActive(false);
						items.Remove(item);
						break;
					}
				}
			}
		}
	}

}
