using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RigidbodyConstraints;
using Random = UnityEngine.Random;

public class Storage : MonoBehaviour {
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
	public int maxItem;
	
	public int normalItem;
	public int rareItem;
	public int epicItem;
	public int legendaryItem;
	[Space] 
	public List<GameObject> normalItems=new List<GameObject>();
	public List<GameObject> rareItems=new List<GameObject>();
	public List<GameObject> epicItems=new List<GameObject>();
	public List<GameObject> legendaryItems=new List<GameObject>();
	[HideInInspector]public Transform currentSputoPoint;
	[HideInInspector]public List<Item> items=new List<Item>();

	public void Init(int floorNumber) {
		StartCoroutine(GenerateInitialItems());
		for (int i = 0; i < floorNumber; i++) {
			GenerateStorage(i);
		}
		SetSputoPoint(0);
	}

	public IEnumerator GenerateInitialItems() {
		for (int i = 0; i < normalItem; i++) {
			yield return StartCoroutine(SpawnItem(RarityLevel.Normal));
		}
		for (int i = 0; i < rareItem; i++) {
			yield return StartCoroutine(SpawnItem(RarityLevel.Rare));
		}
		for (int i = 0; i < epicItem; i++) {
			yield return StartCoroutine(SpawnItem(RarityLevel.Epic));
		}
		for (int i = 0; i < legendaryItem; i++) {
			yield return StartCoroutine(SpawnItem(RarityLevel.Legendary));
		}
	}
	
	public IEnumerator SpawnItem(RarityLevel rarity) {
		GameObject prefab;
		switch (rarity) {
			case RarityLevel.Normal:
				prefab = normalItems[Random.Range(0, normalItems.Count)];
				break;
			case RarityLevel.Rare:
				prefab = rareItems[Random.Range(0, rareItems.Count)];
				break;
			case RarityLevel.Legendary:
				prefab = epicItems[Random.Range(0, epicItems.Count)];
				break;
			case RarityLevel.Epic:
				prefab = legendaryItems[Random.Range(0, legendaryItems.Count)];
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
		}
		Instantiate(prefab.gameObject, spawnPoint.position,spawnPoint.rotation);
		var item = prefab.GetComponent<Item>();
		if (item != null) {
			item.gameObject.name = item.Name;
		}
		yield return new WaitForSeconds(0f);
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

	

}
