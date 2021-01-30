using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public Transform target;
    public Vector3 _offset;

    private void Update()
    {
        this.transform.position = _offset + target.transform.position;
    }
}
