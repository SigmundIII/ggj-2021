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
		
		private Character[] characters;
		private Character[] enemies;

		private Battle battle;

		private TurnSystem turnSystem;

		private bool battleEnded;

		public List<Item> items;

		private void Awake() {
			turnSystem = FindObjectOfType<TurnSystem>();
			turnSystem.OnBattlePhaseStart += StartBattle;
			
			characters = new Character[party.Length];
			for (int i = 0; i < characters.Length; i++) {
				characters[i] = new Character(party[i]);
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
			battle = new Battle(characters, enemies, FindObjectOfType<Storage>().items);
			aftermath.Hide();
			VisualLog.Show();
			StartCoroutine(battle.BattleCoroutine(EndBattle));
		}

		private void EndBattle() {
			battleEnded = true;
			VisualLog.Hide();
			aftermath.Show(characters);
		}
	}

}