using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
	public enum RarityLevel{ Normal, Rare, Legendary, Epic }
	
	public class Item : MonoBehaviour{
		public ItemType Type {  private set; get; }
		public RarityLevel RarityLevel {  private set; get; }
		
		public int MaxHp { private set; get; }
		public int Attack { private set; get; }
		public int Defense { private set; get; }
		
		public int BattleValue { private set; get; }

		public void Generate(ItemType type, RarityLevel rarityLevel) {
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