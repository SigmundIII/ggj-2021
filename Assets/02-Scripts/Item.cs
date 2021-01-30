using UnityEngine;

namespace DefaultNamespace {
	public enum RarityLevel{ Normal, Rare, Legendary, Epic }
	
	public class Item : MonoBehaviour {
		public string Name { get; private set; }
		public ItemType type;
		public RarityLevel rarityLevel = RarityLevel.Normal;
		
		public int MaxHp { private set; get; }
		public int Attack { private set; get; }
		public int Defense { private set; get; }
		
		public int BattleValue { private set; get; }

		public void Awake() {
			Name = rarityLevel + " " + type.Name;

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