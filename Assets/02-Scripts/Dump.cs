using UnityEngine;

namespace DefaultNamespace {

	public class Dump : MonoBehaviour {

		public void AddGarbage(Transform garbage) {
			garbage.SetParent(transform);
			Destroy(garbage.gameObject);
		}
	}

}