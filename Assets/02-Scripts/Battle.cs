using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visual_Log;

namespace DefaultNamespace {

	public class Battle {
		private List<Character> heroes;
		private List<Character> enemies;

		private List<Character> characters;

		private bool someoneDead;

		public Battle(Character[] heroes, Character[] enemies) {
			this.heroes = new List<Character>(heroes);
			this.enemies = new List<Character>(enemies);
			characters = new List<Character>(heroes);
			characters.AddRange(enemies);
		}

		public IEnumerator BattleCoroutine() {
			VisualLog.AddLog("\n\n\nBattle started.");
			yield return new WaitForSeconds(0.35f);
			while (heroes.Count > 0 && enemies.Count > 0) {
				yield return DoRound();
			}
			string winners = heroes.Count > 0 ? "Heroes" : "Monsters";
			VisualLog.AddLog($"Battle ended. {winners} won.");
		}

		private IEnumerator DoRound() {
			VisualLog.AddLog("\n\n--- New round ---.");
			characters.Sort(SortCharacters);
			for (int i = 0; i < characters.Count; i++) {
				yield return DoTurn(characters[i]);
				if (someoneDead) {
					if (enemies.Count <= 0) {
						yield break;
					}
				}
				yield return new WaitForSeconds(0.35f);
			}
		}

		private IEnumerator DoTurn(Character character) {
			VisualLog.AddLog($"\n-- Turn of: {character.name}");
			bool isHero = heroes.Contains(character);
			yield return new WaitForSeconds(0.35f);
			
			Character target;
			if (isHero) {
				int index = Random.Range(0, enemies.Count);
				target = enemies[index];
			} else {
				int index = Random.Range(0, heroes.Count);
				target = heroes[index];
			}
			VisualLog.AddLog($"[{character.name}] attacks {target.name}");
			yield return new WaitForSeconds(0.35f);
			
			int damage = Utility.CalculateDamage(character, target);
			VisualLog.AddLog($"[{character.name}] deals {damage} damage! ");
			target.Hurt(damage);
			VisualLog.AddLog($"[{target.name}] has now {target.Health}/{target.MaxHp}!\n\n");
			if (target.Health <= 0) {
				someoneDead = true;
				characters.Remove(target);
				VisualLog.AddLog($"[{target.name}] Died!");
				if (isHero) {
					enemies.Remove(target);
				} else {
					heroes.Remove(target);
				}
			}
		}

		private static int SortCharacters(Character x, Character y) {
			if (x.BattleValue > y.BattleValue) return -1;
			else if (x.BattleValue < y.BattleValue) return 1;
			else return 0;
		}
	}

}