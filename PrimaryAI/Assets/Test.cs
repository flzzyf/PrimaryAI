using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    

    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.Cross(transform.position, transform.right), 20f);
    }
}
