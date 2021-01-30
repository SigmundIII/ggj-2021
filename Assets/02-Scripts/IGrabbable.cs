using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable {
	void Grabbed(Transform parent);
	void Released();

	void Throw(Vector3 force);

	void Stop();
}
