using UnityEngine;

namespace DefaultNamespace {

	public class GameManagerTMP : MonoBehaviour {
		[SerializeField] private CharacterClass[] party;

		private Character[] characters;

		private Battle battle;

		private void Awake() {
			battle = new Battle();
		}

		private void Start() {
			characters = new Character[party.Length];
			for (int i = 0; i < characters.Length; i++) {
				characters[i] = new Character(party[i]);
			}
			
			StartCoroutine(battle.BattleCoroutine());
		}
	}

}