using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI {

	public class UIAftermath : MonoBehaviour {
		[System.Serializable]
		class CharacterAftermath {
			public TextMeshProUGUI tmproName;
			public TextMeshProUGUI tmproDescription;
			public Transform itemContent;
			public GameObject itemTemplate;
		}

		[SerializeField] private CharacterAftermath[] characterAftermath;

		private void Awake() {
			Hide();
		}

		public void Show(Character[] characters) {
			for (int i = 0; i < characters.Length; i++) {
				characterAftermath[i].tmproName.text = characters[i].name;
				characterAftermath[i].tmproDescription.text = characters[i].ToString();
				
				for (int j = 0; j < characterAftermath[i].itemContent.childCount; j++) {
					Transform child = characterAftermath[i].itemContent.GetChild(j);
					if (child != characterAftermath[i].itemTemplate.transform) {
						Destroy(child.gameObject);
					}
				}
				
				foreach (Item item in characters[i].equipment) {
					if (item != null) {
						GameObject itemObj = Instantiate(characterAftermath[i].itemTemplate, characterAftermath[i].itemContent);
						itemObj.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
						itemObj.SetActive(true);
					}
				}
			}
			
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}

}