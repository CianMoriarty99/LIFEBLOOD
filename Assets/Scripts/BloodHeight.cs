using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodHeight : MonoBehaviour
{

    public float heightMultiplier;

    public Vector3 startingPosition;
    void Start()
    {
        heightMultiplier = 0;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
