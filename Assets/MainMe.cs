using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMe : MonoBehaviour {
	[SerializeField] private float waitTime;
	[SerializeField] private float scrollSpeed;

	private AudioSource audioSource;

	[SerializeField] private GameObject textPanel;
	[SerializeField] private Transform text;

	private void Awake() {
		audioSource = GetComponent<AudioSource>();
	}

	public void StartGame() {
		audioSource.Play();
		textPanel.SetActive(true);
		StartCoroutine(WaitForSong());
	}

	public IEnumerator WaitForSong() {
		float timer = 0;
		while (timer < waitTime) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				break;
			}
			timer += Time.deltaTime;
			text.position += Vector3.up * scrollSpeed * Time.deltaTime;
			yield return null;
		}
		
		SceneManager.LoadScene("00-Scenes/SCENE GIUSTE/SampleScene");
	}
}