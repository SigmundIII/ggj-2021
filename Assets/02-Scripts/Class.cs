using UnityEngine;

namespace DefaultNamespace {

	[CreateAssetMenu(fileName = "Class-", menuName = "New Class", order = 0)]
	public class Class : ScriptableObject {
		[SerializeField] private string _name;
		
		[Header("Base Stats")] 
		[SerializeField] private int maxHp;
		[SerializeField] private int attack;
		[SerializeField] private int defense;
		
		[Header("Equipment")]
		[SerializeField] private ItemType[] equipableTypes;
		
		public string Name => _name;

		public int MAXHp => maxHp;
		public int Attack => attack;
		public int Defense => defense;
		
		public ItemType[] EquipableTypes => equipableTypes;
	}
}