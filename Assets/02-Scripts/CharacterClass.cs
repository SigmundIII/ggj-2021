using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {

	[CreateAssetMenu(fileName = "Class-", menuName = "New Class", order = 0)]
	public class CharacterClass : ScriptableObject {
		[Serializable]
		public class Slot {
			public List<ItemType> types;
			[HideInInspector] public Item equipped;
		}
		
		[SerializeField] private string _name;
		
		[Header("Base Stats")] 
		[SerializeField] private int maxHp;
		[SerializeField] private int attack;
		[SerializeField] private int defense;
		
		[Header("Equipment")]
		[SerializeField] private Slot[] equipSlots;
		
		public string Name => _name;

		public int MAXHp => maxHp;
		public int Attack => attack;
		public int Defense => defense;
		
		public Slot[] EquipSlots => equipSlots;
	}
}