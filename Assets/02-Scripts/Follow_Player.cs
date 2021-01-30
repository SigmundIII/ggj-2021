using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public Transform target;
    public float zoom;

    private void FixedUpdate() {
        // Vector3 direction = target.position-transform.position ;
        // RaycastHit hit;
        // if(Physics.Raycast(transform.position,direction,out hit,Vector3.Distance(target.position,transform.position))
        // {
        //     MeshRenderer mesh = hit.transform.gameObject.GetComponent<MeshRenderer>();
        //     
        //
        // }
    }

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
        hits = Physics.RaycastAll(transform.position, target.position-transform.position, Vector3.Distance(transform.position,target.position));
        foreach(RaycastHit hit in hits)
        {
            MeshRenderer R = hit.collider.GetComponent<MeshRenderer>();
            if (R == null)
                continue; // no renderer attached? go to next hit
            // TODO: maybe implement here a check for GOs that should not be affected like the player

            
            
            R.enabled = false;
            //
            // AutoTransparent AT = R.GetComponent<AutoTransparent>();
            // if (AT == null) // if no script is attached, attach one
            // {
            //     AT = R.gameObject.AddComponent<AutoTransparent>();
            // }
            // AT.BeTransparent(); // get called every frame to reset the falloff
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position,(target.position-transform.position)*5);
    }
}
