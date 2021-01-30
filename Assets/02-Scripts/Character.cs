using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {

	public class Character {
		public string name;
		
		public Item[] equipment;

		public CharacterClass Class { get; }

		public int MaxHp { get; private set; }
		public int Attack { get; private set; }
		public int Defense { get; private set; }

		public int BattleValue { get; private set; }

		public int Health { get; private set; }

		public Character(CharacterClass characterClass) {
			Class = characterClass;
			name = Class.Name;
			
			Health = MaxHp = Class.MAXHp;
			Attack = Class.Attack;
			Defense = Class.Defense;

			BattleValue += MaxHp + Attack + Defense;
			
			equipment = new Item[characterClass.EquipSlots.Length];
		}

		public void Hurt(int damage) {
			Health -= damage;
			if (Health <= 0) {
				Health = 0;
			}
		}

		public bool CanEquip(Item item) {
			return Utility.EquipableTypeIndex(Class, item.type) >= 0;
		}

		public bool Equip(Item item) {
			if (!CanEquip(item)) {
				return false;
			}
			
			var equipmentList = new List<Item>(equipment);
			Item previousEquip = equipmentList.Find(i => i != null && i.type == item.type);
			if (previousEquip != null && item.BattleValue <= previousEquip.BattleValue) {
				return false;
			} else {
				int index = Utility.EquipableTypeIndex(Class, item.type);
				equipment[index] = item;
				MaxHp += previousEquip != null ? item.MaxHp - previousEquip.MaxHp : item.MaxHp;
				Health += previousEquip != null ? item.MaxHp - previousEquip.MaxHp : item.MaxHp;
				Attack += previousEquip != null ? item.Attack - previousEquip.Attack : item.Attack;
				Defense += previousEquip != null ? item.Defense - previousEquip.Defense : item.Defense;
				BattleValue += previousEquip != null ? item.BattleValue - previousEquip.BattleValue : item.BattleValue;
				return true;
			}
		}

		public override string ToString() {
			return $"Class: {Class.Name}\n" +
			       $"Battle Value: {BattleValue}\n" +
			       $"\n" +
			       $"Hp: {Health}/{MaxHp}\n" +
			       $"Attack: {Attack}\n" +
			       $"Defense: {Defense}";
		}
	}

}