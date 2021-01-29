using UnityEngine;

namespace DefaultNamespace {

	[CreateAssetMenu(fileName = "ItemType-", menuName = "New ItemType", order = 0)]
	public class ItemType : ScriptableObject {
		[SerializeField] private string _name;
		[Space]
		[SerializeField] private Vector2 maxHpRange;
		[SerializeField] private Vector2 attackRange;
		[SerializeField] private Vector2 defenseRange;

		public string Name => _name;
		public Vector2 MAXHpRange => maxHpRange;
		public Vector2 AttackRange => attackRange;
		public Vector2 DefenseRange => defenseRange;
	}

}