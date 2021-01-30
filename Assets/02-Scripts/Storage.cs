using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public class Storage : MonoBehaviour {
	public GameObject prefab;
	public Transform spawnPoint;

	public GameObject sputoPointPrefab;
	public GameObject storageDoorPrefab;
	public GameObject storageWallsPrefab;
	public GameObject storageFloorPrefab;
	
	
	[Space]
	private List<Transform> sputoPoint=new List<Transform>();
	private List<GameObject> storageDoors=new List<GameObject>();
	private List<GameObject> storageWalls=new List<GameObject>();
	private List<GameObject> storageFloors=new List<GameObject>();
	
	[Space]
	public float sputoForce;
	public int maxItem;
	
	public int normalItem;
	public int rareItem;
	public int epicItem;
	public int legendaryItem;

	private Transform currentSputoPoint;
	public List<ItemType> types=new List<ItemType>();
	private List<Item> items=new List<Item>();

	public void Init(int floorNumber) {
		StartCoroutine(GenerateInitialItems());
		for (int i = 0; i < floorNumber; i++) {
			GenerateStorage(i);
		}
		SetSputoPoint(0);
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
		if (item != null) {
			item.Generate(type,rarity);
			item.gameObject.name = "Item";
		}
		yield return new WaitForSeconds(0.5f);
	}

	public void GenerateStorage(int floorNumber) {
		GameObject floor=new GameObject();
		floor.name = "Floor" + floorNumber;
		var position = transform.position;
		position.y -= (9 * floorNumber);
		var obj = Instantiate(storageWallsPrefab, position, Quaternion.identity);
		obj.transform.parent = floor.transform;
		storageWalls.Add(obj);
		obj = Instantiate(storageFloorPrefab, position, Quaternion.identity);
		storageFloors.Add(obj);
		obj.transform.parent = floor.transform;
		obj = Instantiate(storageDoorPrefab, position, Quaternion.identity);
		storageDoors.Add(obj);
		obj.transform.parent = floor.transform;
		obj = Instantiate(sputoPointPrefab, position, Quaternion.identity);
		obj.transform.GetChild(0);
		sputoPoint.Add(obj.transform.GetChild(0));
		obj.transform.parent = floor.transform;
		floor.transform.parent = transform;
	}
	
	
	public void DestroyFloor(int currentFloor) {
		Destroy(storageFloors[currentFloor]);
	}
	public void DestroyWalls(int currentFloor) {
		Destroy(storageWalls[currentFloor]);
	}
	public void DestroyDoors(int currentFloor) {
		Destroy(storageDoors[currentFloor]);
	}

	public void OpenDoors(int currentFloor) {
		storageDoors[currentFloor].SetActive(false);
	}
	
	public void CloseDoors(int currentFloor) {
		storageDoors[currentFloor].SetActive(true);
	}

	public void SetSputoPoint(int currentFloor) {
		currentSputoPoint = sputoPoint[currentFloor];
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
					grabbable.Released();
					other.transform.position = currentSputoPoint.position;
					// Vector3 dir = currentSputoPoint.position - other.gameObject.transform.position;
					// grabbable.Throw( dir*sputoForce);
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
