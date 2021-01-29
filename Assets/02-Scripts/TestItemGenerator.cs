using UnityEngine;

namespace DefaultNamespace {

	public class TestItemGenerator : MonoBehaviour {
		[SerializeField] private ItemType itemType;
		[SerializeField] private RarityLevel rarityLevel;

		public void Generate() {
			var item = new Item();
			item.Generate(itemType,rarityLevel);
			Debug.Log($"Type: {itemType.Name}\n" +
			          $"Rarity: {item.RarityLevel}\n" +
			          $"Battle Value: {item.BattleValue}\n\n" +
			          $"MaxHp: {item.MaxHp}\n" +
			          $"Attack: {item.Attack}\n" +
			          $"Defense: {item.Defense}");
		}
	}

}