using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class StoreObjectManager : MonoBehaviour {
    private Storage storage;

    private Transform storagedParent;
    
    private void Awake() {
        storage = FindObjectOfType<Storage>();
        storagedParent = new GameObject("Storaged Items - " + name).transform;
    }

    private void OnTriggerEnter(Collider other) {
        Item item=other.GetComponent<Item>();
        if (item != null) {
            if (storage.items.Count < storage.maxItem) {
                storage.items.Add(item);
                if (!item.gameObject.GetComponent<BasicObject>().grabbed) {
                    item.transform.SetParent(storagedParent);
                }
            }
            else {
                IGrabbable grabbable = other.GetComponent<IGrabbable>();
                if (grabbable!=null) {
                    grabbable.Released();
                    grabbable.Stop();
                    other.transform.position = storage.currentSputoPoint.position;
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
            storage.items.Remove(item);
        }
    }
}
