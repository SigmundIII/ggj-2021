using UnityEngine;

namespace DefaultNamespace {

	public class Dump : MonoBehaviour {

		public void AddGarbage(Transform garbage) {
			garbage.SetParent(transform);
			garbage.gameObject.SetActive(false);
			Destroy(garbage.gameObject);
		}

		private void OnTriggerEnter(Collider other) {
			other.gameObject.GetComponent<BasicObject>()?.DisappearInTheVoid();
			AddGarbage(other.transform);
		}
	}

}