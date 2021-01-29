using System.Collections.Generic;

namespace DefaultNamespace {

	public class Character {
		public CharacterClass Class { get; }

		public int MaxHp { get; private set; }
		public int Attack { get; private set; }
		public int Defense { get; private set; }

		public int BattleValue { get; private set; }

		private Item[] equipment;

		public Character(CharacterClass characterClass) {
			Class = characterClass;
			
			MaxHp = Class.MAXHp;
			Attack = Class.Attack;
			Defense = Class.Defense;
			
			equipment = new Item[characterClass.EquipableTypes.Length];
		}

		public bool CanEquip(Item item) {
			return Utility.EquipableTypeIndex(Class, item.Type) >= 0;
		}

		public bool Equip(Item item) {
			if (!CanEquip(item)) {
				return false;
			}
			
			var equipmentList = new List<Item>(equipment);
			Item previousEquip = equipmentList.Find(i => i.Type == item.Type);
			if (previousEquip != null && item.BattleValue < previousEquip.BattleValue) {
				return false;
			} else {
				int index = Utility.EquipableTypeIndex(Class, item.Type);
				equipment[index] = item;
				MaxHp += previousEquip != null ? item.MaxHp - previousEquip.MaxHp : item.MaxHp;
				Attack += previousEquip != null ? item.Attack - previousEquip.Attack : item.Attack;
				Defense += previousEquip != null ? item.Defense - previousEquip.Defense : item.Defense;
				BattleValue += previousEquip != null ? item.BattleValue - previousEquip.BattleValue : item.BattleValue;
				return true;
			}
		}
	}

}