using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Storage : MonoBehaviour {
	public GameObject prefab;
	public Transform spawnPoint;

	public ItemType type;
	public RarityLevel rarity;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.G)) {
			var item = prefab.GetComponent<Item>();
			if (item != null) {
				item.Generate(type,rarity);
				Instantiate(prefab, spawnPoint.position,spawnPoint.rotation);
			}
			else {
				Debug.LogError("Non stai spawnando un item");
			}

		}
	}
}
