using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDungeon : MonoBehaviour {
    public GameObject dungeonFloorPrefab;

    private List<GameObject> dungeonFloors=new List<GameObject>();
    
    public void Init(int maxFloor) {
        for (int i = 0; i < maxFloor; i++) {
            GenerateDungeon(i);
        }
    }
    
    public void GenerateDungeon(int floorNumber) {
        GameObject dungeonFloor=new GameObject();
        dungeonFloor.name = "Dungeon " + floorNumber;
        var position = transform.position;
        position.y -= (9 * floorNumber);
        var obj = Instantiate(dungeonFloorPrefab, position, Quaternion.identity);
        obj.transform.parent = dungeonFloor.transform;
        dungeonFloors.Add(obj);
        dungeonFloor.transform.parent = transform;
    }

    public void DestroyFloor(int currentFloor) {
        Destroy(dungeonFloors[currentFloor]);
    }
}
