using UnityEngine;

namespace DefaultNamespace {

	public class GameManagerTMP : MonoBehaviour {
		[SerializeField] private CharacterClass[] party;
		[SerializeField] private CharacterClass[] enemyParty;

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
			battle = new Battle(characters, enemies);
			StartCoroutine(battle.BattleCoroutine());
		}
	}

}