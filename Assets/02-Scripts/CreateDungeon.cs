using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDungeon : MonoBehaviour {
    public GameObject dungeonFloorPrefab;
    public GameObject bossDungeonPrefab;

    private List<GameObject> dungeonFloors=new List<GameObject>();
    
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
        position.y -= (9 * floorNumber);
        var obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.parent = dungeonFloor.transform;
        dungeonFloors.Add(obj);
        dungeonFloor.transform.parent = transform;
    }

    public void DestroyFloor(int currentFloor) {
        Destroy(dungeonFloors[currentFloor]);
    }
}
