using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Storage : MonoBehaviour {
	public Transform spawnPoint;
	public Transform sputoPoint;
	
	public GameObject storagePrefab;

	[Space] public int floorHeight = 9;
	public List<GameObject> storageAllFloor=new List<GameObject>();

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
	public List<Item> items = new List<Item>();

	[HideInInspector] public Transform treasonPoint;

	public void Init(int floorNumber) {
		for (int i = 0; i < floorNumber; i++) {
			GenerateFloor(i);
		}
		StartCoroutine(GenerateInitialItems());
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			MoveFloors();
		}
	}

	public IEnumerator GenerateInitialItems() {
		yield return null;
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
		yield return new WaitForSeconds(2);
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
		
		GameObject obj = Instantiate(prefab, spawnPoint.position,spawnPoint.rotation);
		var item = obj.GetComponent<Item>();
		if (item != null) {
			item.gameObject.name = item.Name;
			AddItem(item);
		}
		yield return new WaitForSeconds(0f);
	}

	public void AddItem(Item it) {
		if (items.Count < maxItem) {
			items.Add(it);
		}
		else {
			it.transform.position = sputoPoint.transform.position;
		}
	}

	public void GenerateFloor(int floorNumber) {
		GameObject floor=new GameObject();
		floor.name = "Floor" + floorNumber;
		var position = transform.position;
		position.y = -floorHeight*floorNumber;
		var obj = Instantiate(storagePrefab, position, Quaternion.identity);
		obj.transform.SetParent(floor.transform);
		storageAllFloor.Add(obj);
		var pieces=storagePrefab.GetComponent<StoragePieces>();
		if (pieces != null) {
			treasonPoint = pieces.treasonPoint;
		}
	}

	public void MoveFloors() {
		foreach (GameObject obj in storageAllFloor) {
			if (obj != null) {
				obj.transform.position=new Vector3(obj.transform.position.x,obj.transform.position.y+floorHeight,obj.transform.position.z);
			}
		}
	}

	public void DestroyAllFloor(int currentFloor) {
		Destroy(storageAllFloor[currentFloor]);
	}

	public void NextFloor(int currentFloor) {
		DestroyAllFloor(currentFloor);
		MoveFloors();
	}
	
}
