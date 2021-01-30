using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DungeonItemsManager : MonoBehaviour
{
    private GameManager storage;
    private void Awake() {
        storage = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other) {
        Item item=other.GetComponent<Item>();
        if (item != null) {
            storage.items.Add(item);
        }
    }
	
    private void OnTriggerExit(Collider other) {
        Item item=other.GetComponent<Item>();
        if (item != null) {
            storage.items.Remove(item);
        }
    }
}
