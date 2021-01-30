using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;
using Visual_Log;

namespace DefaultNamespace {

	public class GameManager : MonoBehaviour {
		[SerializeField] private CharacterClass[] party;
		[SerializeField] private CharacterClass[] enemyParty;
		[Space]
		[SerializeField] private UIAftermath aftermath;
		
		[HideInInspector]
		public Character[] heroes;
		private Character[] enemies;

		private Battle battle;

		private TurnSystem turnSystem;
		
		private bool battleEnded;

		private Dump dump;

		[HideInInspector] public List<Item> items;

		private Ritual_affordance _ritual;

		private void Awake() {
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

		public void DestroyItems() {
			foreach (Item item  in items) {
				Destroy(item.gameObject);
			}
			items.Clear();
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.R)) {
				StartBattle();
			}

			if (Input.GetKeyDown(KeyCode.N)) {
				switch (turnSystem.currentPhase) {
				case TurnPhase.Battle when battleEnded:
					aftermath.Hide();
					turnSystem.NextTurn();
					break;
				case TurnPhase.Loot:
					for (int i = 0; i < items.Count; i++) {
						dump.AddGarbage(items[i].transform);
						items.RemoveAt(i);
					}
					break;
				}
			}
		}

		private void StartBattle() {
			battleEnded = false;
			battle = new Battle(heroes, enemies);
			aftermath.Hide();
			VisualLog.Show();
			StartCoroutine(battle.BattleCoroutine(EndBattle));
		}

		private void EndBattle() {
			battleEnded = true;
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