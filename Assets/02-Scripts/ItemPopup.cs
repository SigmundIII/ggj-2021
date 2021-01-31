using TMPro;
using UnityEngine;

namespace DefaultNamespace {

	public class ItemPopup : MonoBehaviour {
		private static ItemPopup instance;

		[SerializeField] private TextMeshProUGUI itemName;
		[SerializeField] private TextMeshProUGUI hp;
		[SerializeField] private TextMeshProUGUI atk;
		[SerializeField] private TextMeshProUGUI def;
		[SerializeField] private TextMeshProUGUI bl;

		private void Awake() {
			if (instance != null) {
				Destroy(gameObject);
			} else {
				instance = this;
			}
		}

		private void Start() {
			_Hide();
		}

		private void _Hide() {
			gameObject.SetActive(false);
		}

		private void _Show(Item item) {
			gameObject.SetActive(true);
			transform.position = item.transform.position;
			itemName.text = item.Name;
			hp.text = item.MaxHp.ToString();
			atk.text = item.Attack.ToString();
			def.text = item.Defense.ToString();
			bl.text = item.BattleValue.ToString();
		}

		public static void Hide() {
			instance._Hide();
		}

		public static void Show(Item item) {
			instance._Show(item);
		}
	}

}