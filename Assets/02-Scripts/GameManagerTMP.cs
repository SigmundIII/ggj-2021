using DefaultNamespace.UI;
using UnityEngine;
using Visual_Log;

namespace DefaultNamespace {

	public class GameManagerTMP : MonoBehaviour {
		[SerializeField] private CharacterClass[] party;
		[SerializeField] private CharacterClass[] enemyParty;
		[Space]
		[SerializeField] private UIAftermath aftermath;

		private Character[] characters;
		private Character[] enemies;

		private Battle battle;

		private void Awake() {
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
			StartBattle();
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.R)) {
				StartBattle();
			}
		}

		private void StartBattle() {
			battle = new Battle(characters, enemies);
			aftermath.Hide();
			VisualLog.Show();
			StartCoroutine(battle.BattleCoroutine(EndBattle));
		}

		private void EndBattle() {
			VisualLog.Hide();
			aftermath.Show(characters);
		}
	}

}