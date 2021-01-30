using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Visual_Log;

namespace DefaultNamespace {

	public class GameManager : MonoBehaviour {
		[SerializeField] private CharacterClass[] party;
		[SerializeField] private CharacterClass[] enemyParty;
		[Space]
		[SerializeField] public UIAftermath aftermath;
		
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
			
			enemies = new Character[enemyParty.Length];
			for (int i = 0; i < enemies.Length; i++) {
				enemies[i] = new Character(enemyParty[i]);
				enemies[i].name += $" {i}";
			}
		}

		private void Start() {
			VisualLog.Hide();
		}


		private void Update() {
			if (_battleEnded && Input.GetKeyDown(KeyCode.Return)) {
				battleEnded = true;
			}
		}

		public void DumpLoot() {
			for (int i = 0; i < items.Count; i++) {
				dump.AddGarbage(items[i].transform);
				items.RemoveAt(i);
			}
		}

		private void StartBattle() {
			_battleEnded = false;
			battleEnded = false;
			battle = new Battle(heroes, enemies);
			aftermath.Hide();
			VisualLog.Show();
			StartCoroutine(battle.BattleCoroutine(EndBattle));
		}

		private void EndBattle() {
			_battleEnded = true;
			VisualLog.Hide();
			AssignLoot();
			aftermath.Show(heroes);
			//_ritual.StartCoroutine("Ritual_progression");
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