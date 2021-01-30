using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public Transform target;
    public float zoom;

    void Update() {
        Vector3 targetpos=Vector3.zero;
        Vector3 dir = transform.forward;
        dir.Normalize();
        targetpos = target.position+(-dir * zoom);
        
        // Move the camera smoothly to the target position
        transform.position = targetpos;
    }
}
