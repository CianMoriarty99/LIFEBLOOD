using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLevelManager : MonoBehaviour
{
    public int powerLevel;
    CardController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponentInParent<CardController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
