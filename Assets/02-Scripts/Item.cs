using UnityEngine;

namespace DefaultNamespace {
	public enum RarityLevel { Normal, Rare, Legendary, Epic }
	
	public class Item {
		public ItemType Type { get; }
		public RarityLevel RarityLevel { get; }
		
		public int MaxHp { get; }
		public int Attack { get; }
		public int Defense { get; }
		
		public int BattleValue { get; }
		
		public Item(ItemType type, RarityLevel rarityLevel) {
			Type = type;
			RarityLevel = rarityLevel;

			Vector2 hpr = type.MAXHpRange * Utility.RarityLevelMultiplier(rarityLevel);
			Vector2 attr = type.AttackRange * Utility.RarityLevelMultiplier(rarityLevel);
			Vector2 defr = type.DefenseRange * Utility.RarityLevelMultiplier(rarityLevel);

			MaxHp = (int) Random.Range(hpr.x, hpr.y);
			Attack = (int) Random.Range(attr.x, attr.y);
			Defense = (int) Random.Range(defr.x, defr.y);

			BattleValue = Utility.CalculateItemBattleValue(this);
		}
	}
}