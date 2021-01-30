using UnityEngine;

namespace DefaultNamespace {

	public class Dump : MonoBehaviour {

		public void AddGarbage(Transform garbage) {
			Debug.Log("Dumped " + garbage.name);
			garbage.SetParent(transform);
			Destroy(garbage.gameObject);
		}
	}

}