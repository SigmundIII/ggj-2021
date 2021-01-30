using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public class Storage : MonoBehaviour {
	public GameObject prefab;
	public Transform spawnPoint;
	[Space]
	public List<Transform> sputoPoint;
	public List<GameObject> storageDoors;
	public List<GameObject> storageFloors;
	[Space]
	public float sputoForce;
	public int maxItem;
	
	public int normalItem;
	public int rareItem;
	public int epicItem;
	public int legendaryItem;

	private Transform currentSputoPoint;
	private List<ItemType> types=new List<ItemType>();
	private List<Item> items=new List<Item>();

	private void Awake() {
		LoadTypes();
	}
	
	public void Init() {
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

	public void DestroyFloor(int currentFloor) {
		Destroy(storageFloors[currentFloor]);
	}

	public void OpenDoors(int currentFloor) {
		storageDoors[currentFloor].SetActive(false);
	}
	
	public void CloseDoors(int currentFloor) {
		storageDoors[currentFloor].SetActive(true);
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
					Vector3 dir = currentSputoPoint.position - other.gameObject.transform.position;
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
