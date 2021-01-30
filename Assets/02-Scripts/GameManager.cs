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
		
		private Character[] heroes;
		private Character[] enemies;

		private Battle battle;

		private TurnSystem turnSystem;
		
		private bool battleEnded;

		[HideInInspector] public List<Item> items;

		private void Awake() {
			turnSystem = FindObjectOfType<TurnSystem>();
			turnSystem.OnBattlePhaseStart += StartBattle;
			
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

			if (battleEnded && Input.GetKeyDown(KeyCode.N)) {
				aftermath.Hide();
				turnSystem.NextTurn();
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
		}

		private void AssignLoot() {
			var loot = new List<Item>(items);
			// string log = "";
			// foreach (Item item in loot) {
			// 	log += $"{item.Name} - {item.BattleValue}\n";
			// }
			// Debug.Log(log);
			var _heroes = new List<Character>(heroes);
			_heroes.Sort(Utility.SortCharacters);
			_heroes.Reverse();
			for (int i = 0; i < loot.Count; i++) {
				Item item = loot[i];
				foreach (Character hero in _heroes) {
					if (hero.CanEquip(item) && hero.Equip(item)) {
						// Debug.Log($"{hero.name} equipped {item.Name}");
						loot.Remove(item);
						break;
					}
				}
			}
		}
	}

}