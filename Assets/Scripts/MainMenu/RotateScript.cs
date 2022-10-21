using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float zPerSec, xPerSec, yPerSec;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.Rotate(xPerSec,yPerSec,zPerSec);
    }
}