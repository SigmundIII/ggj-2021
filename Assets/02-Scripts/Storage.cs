using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public class Storage : MonoBehaviour {
	public GameObject prefab;
	public Transform spawnPoint;
	public Transform sputoPoint;
	public float sputoForce;
	public int maxItem;
	
	public int normalItem;
	public int rareItem;
	public int epicItem;
	public int legendaryItem;
	
	private List<ItemType> types=new List<ItemType>();
	private List<Item> items=new List<Item>();

	private void Awake() {
		LoadTypes();
		StartCoroutine(GenerateInitialItems());
	}

	private void LoadTypes() {
		string[] guids = AssetDatabase.FindAssets("t:ItemType");
		foreach (var guid in guids) {
			types.Add(AssetDatabase.LoadAssetAtPath<ItemType>(AssetDatabase.GUIDToAssetPath(guid)));
		}
	}

	public IEnumerator GenerateInitialItems() {
		for (int i = 0; i < normalItem; i++) {
			foreach (ItemType type in types) {
				yield return StartCoroutine(SpawnItem(RarityLevel.Normal, type));
			}
		}
		for (int i = 0; i < rareItem; i++) {
			foreach (ItemType type in types) {
				yield return StartCoroutine(SpawnItem(RarityLevel.Rare, type));
			}
		}
		for (int i = 0; i < epicItem; i++) {
			foreach (ItemType type in types) {
				yield return StartCoroutine(SpawnItem(RarityLevel.Epic, type));
			}
		}
		for (int i = 0; i < legendaryItem; i++) {
			foreach (ItemType type in types) {
				yield return StartCoroutine(SpawnItem(RarityLevel.Legendary, type));
			}
		}
	}
	
	public IEnumerator SpawnItem(RarityLevel rarity, ItemType type) {
		Instantiate(prefab.gameObject, spawnPoint.position,spawnPoint.rotation);
		Item item = prefab.GetComponent<Item>();
		item.Generate(type,rarity);
		item.gameObject.name = "Item";
		yield return new WaitForSeconds(0.5f);
	}
	
	private void Update() {
		// if (Input.GetKeyDown(KeyCode.G)) {
		// 	var item = prefab.GetComponent<Item>();
		// 	if (item != null) {
		// 		item.Generate(types[0],rarities[0]);
		// 		Instantiate(prefab, spawnPoint.position,spawnPoint.rotation);
		// 	}
		// 	else {
		// 		Debug.LogError("Non stai spawnando un item");
		// 	}
		//
		// }
	}

	private void OnTriggerEnter(Collider other) {
		Item item=other.GetComponent<Item>();
		if (item != null) {
			if (items.Count < maxItem) {
				items.Add(item);
			}
			else {
				IGrabbable grabbable = other.GetComponent<IGrabbable>();
				if (grabbable!=null) {
					Vector3 dir = sputoPoint.position - other.gameObject.transform.position;
					grabbable.Throw( dir*sputoForce);
					//other.transform.position = sputoPoint.position;
				}
			}
		}
	}
	
	private void OnTriggerExit(Collider other) {
		Item item=other.GetComponent<Item>();
		if (item != null) {
			items.Remove(item);
		}
	}
}
