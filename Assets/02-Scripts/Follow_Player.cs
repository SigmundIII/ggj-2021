using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public Transform target;
    public float zoom;

    public List<MeshRenderer> disabledObjects=new List<MeshRenderer>();

    public LayerMask hide;
    
    void Update() {
        Vector3 targetpos=Vector3.zero;
        Vector3 dir = transform.forward;
        dir.Normalize();
        targetpos = target.position+(-dir * zoom);
        // Move the camera smoothly to the target position
        transform.position = targetpos;
        
        RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
        hits = Physics.BoxCastAll(transform.position,Vector3.one, target.position-transform.position,Quaternion.identity, Vector3.Distance(transform.position,target.position), hide);
        foreach(RaycastHit hit in hits)
        {
            MeshRenderer R = hit.collider.GetComponent<MeshRenderer>();
            if (R == null)
                continue; // no renderer attached? go to next hit
            // TODO: maybe implement here a check for GOs that should not be affected like the player
            if (!disabledObjects.Contains(R)) {
                disabledObjects.Add(R);
            }
        }
        try {
            foreach (MeshRenderer render in disabledObjects) {
                if (ContainsMeshRenderer(hits, render)) {
                    render.enabled = false;
                }
                else {
                    render.enabled = true;
                }
            }
        }
        catch (Exception e) {
            ClearList();
        }
    }

    public void ClearList() {
        disabledObjects.Clear();
    }

    public bool ContainsMeshRenderer(RaycastHit[] hits, MeshRenderer renderer) {
        foreach(RaycastHit hit in hits)
        {
            MeshRenderer R = hit.collider.GetComponent<MeshRenderer>();
            if (renderer == R) {
                return true;
            }
        }
        return false;
    }

}
