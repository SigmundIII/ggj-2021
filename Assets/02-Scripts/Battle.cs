using System.Collections;
using UnityEngine;

namespace DefaultNamespace {

	public class Battle {
		struct Enemy {
			public int hp;
			public int attack;
			public int defense;
			public int BattleValue => hp + attack + defense;
		}

		private Character[] characters;
		private Enemy[] enemies;

		public Battle() {
			enemies = new Enemy[4];
			for (int i = 0; i < enemies.Length; i++) {
				enemies[i] = new Enemy();
				enemies[i].attack = 10;
				enemies[i].defense = 10;
				enemies[i].hp = 15;
			}
		}

		public IEnumerator BattleCoroutine() {
			VisualLog.VisualLog.AddLog("Battle started.");
			yield return new WaitForSeconds(0.2f);
			VisualLog.VisualLog.AddLog("First round.");
			
		}
	}

}