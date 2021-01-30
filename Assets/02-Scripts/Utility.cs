using UnityEngine;

namespace DefaultNamespace {

	public static class Utility {

		public static float RarityLevelMultiplier(RarityLevel level) {
			switch (level) {
			default:
			case RarityLevel.Normal: return 1f;
			case RarityLevel.Rare: return 1.25f;
			case RarityLevel.Legendary: return 1.7f;
			case RarityLevel.Epic: return 2f;
			}
		}

		public static int CalculateItemBattleValue(Item item) {
			int hp = Mathf.Abs(item.MaxHp);
			int att = Mathf.Abs(item.Attack);
			int def = Mathf.Abs(item.Defense);
			float rarity = RarityLevelMultiplier(item.rarityLevel);
			return (int) ((hp + att + def) * rarity);
		}

		public static int CalculateDamage(Character a, Character d) {
			return a.Attack - a.Attack * d.Defense / 100;
		}
		
		public static int SortCharacters(Character x, Character y) {
			if (x.BattleValue > y.BattleValue) return -1;
			else if (x.BattleValue < y.BattleValue) return 1;
			else return 0;
		}

		public static int EquipableTypeIndex(CharacterClass characterClass, ItemType type) {
			for (int i = 0; i < characterClass.EquipSlots.Length; i++) {
				if (characterClass.EquipSlots[i].types.Contains(type)) {
					return i;
				}
			}
			
			// Debug.LogError("Item Type not found.\n" +
			//                $"Class: {characterClass.Name}\n" +
			//                $"ItemType: {type.Name}");
			return -1;
		}
	}

}