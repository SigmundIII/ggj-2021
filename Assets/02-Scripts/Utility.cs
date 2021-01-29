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
			return (int) ((item.MaxHp + item.Attack + item.Defense) * RarityLevelMultiplier(item.RarityLevel));
		}
	}

}