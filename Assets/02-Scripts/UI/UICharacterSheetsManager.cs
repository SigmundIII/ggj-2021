using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI {

	public class UICharacterSheetsManager : MonoBehaviour {
		private GameManager gameManager;
		
		[SerializeField] private UICharacterSheet characterSheet;

		[SerializeField] private Button closeSheet;
		[SerializeField] private Button[] characterButtons;

		private void Awake() {
			gameManager = FindObjectOfType<GameManager>();
			
			closeSheet.onClick.AddListener(CloseSheet);
			
			for (int i = 0; i < characterButtons.Length; i++) {
				int index = i;
				characterButtons[i].onClick.AddListener(() => ShowCharacter(characterButtons[index].transform.position, gameManager.heroes[index]));
				characterButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = gameManager.heroes[i].name;
			}
		}

		private void CloseSheet() {
			characterSheet.Close();
		}

		private void ShowCharacter(Vector3 position, Character character) {
			characterSheet.transform.position = position;
			characterSheet.Open(character);
		}

		public void Show() {
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}

}