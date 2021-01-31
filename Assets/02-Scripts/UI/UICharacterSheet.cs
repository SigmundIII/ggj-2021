using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI {

	public class UICharacterSheet : MonoBehaviour {
		public TextMeshProUGUI tmproName;
		public TextMeshProUGUI tmproDescription;
		public Transform itemContent;
		public GameObject itemTemplate;

		private Vector3 velocity;

		private Vector3 ScreenCenter => new Vector3(Screen.width / 2, Screen.height / 2);

		private void Update() {
			if (Vector3.Distance(transform.position, ScreenCenter) > 1) {
				transform.position = Vector3.SmoothDamp(transform.position, ScreenCenter, ref velocity, 0.2f);
			}
		}

		public void Open(Character character) {
			gameObject.SetActive(true);
			tmproName.text = character.name;
			tmproDescription.text = character.ToString();
				
			for (int j = 0; j < itemContent.childCount; j++) {
				Transform child = itemContent.GetChild(j);
				if (child != itemTemplate.transform) {
					Destroy(child.gameObject);
				}
			}
			
			foreach (Item item in character.equipment) {
				if (item != null) {
					GameObject itemObj = Instantiate(itemTemplate, itemContent);
					itemObj.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
					itemObj.SetActive(true);
				}
			}
		}

		public void Close() {
			gameObject.SetActive(false);
		}
	}

}