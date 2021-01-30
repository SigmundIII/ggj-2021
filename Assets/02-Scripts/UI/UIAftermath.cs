using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI {

	public class UIAftermath : MonoBehaviour {
		[System.Serializable]
		class CharacterAftermath {
			public TextMeshProUGUI tmproName;
			public TextMeshProUGUI tmproDescription;
		}

		[SerializeField] private CharacterAftermath[] characterAftermath;

		private void Awake() {
			Hide();
		}

		public void Show(Character[] characters) {
			for (int i = 0; i < characters.Length; i++) {
				characterAftermath[i].tmproName.text = characters[i].name;
				characterAftermath[i].tmproDescription.text = characters[i].ToString();
			}
			
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}

}