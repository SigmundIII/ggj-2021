using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDungeon : MonoBehaviour {
    public GameObject dungeonFloorPrefab;
    public GameObject bossDungeonPrefab;

    private List<GameObject> dungeonFloors=new List<GameObject>();
    public float dungeonHeight;
    
    public void Init(int maxFloor) {
        for (int i = 0; i < maxFloor; i++) {
            if (i == maxFloor - 1) {
                GenerateDungeon(i,bossDungeonPrefab);
            }
            else {
                GenerateDungeon(i,dungeonFloorPrefab);
            }
        }
    }
    
    public void GenerateDungeon(int floorNumber,GameObject prefab) {
        GameObject dungeonFloor=new GameObject();
        dungeonFloor.name = "Dungeon " + floorNumber;
        var position = transform.position;
        position.y -= (dungeonHeight * floorNumber);
        var obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.parent = dungeonFloor.transform;
        dungeonFloors.Add(obj);
        dungeonFloor.transform.parent = transform;
    }

    public void MoveFloor() {
        foreach (GameObject obj in dungeonFloors) {
            if (obj != null) {
                obj.transform.position=new Vector3(obj.transform.position.x,obj.transform.position.y+dungeonHeight,obj.transform.position.z);
            }
        }
    }


    public void NextFloor(int currentFloor) {
        Destroy(dungeonFloors[currentFloor]);
        MoveFloor();
    }
}
