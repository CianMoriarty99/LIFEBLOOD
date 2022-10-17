using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public float xDir, yDir, cameraMoveSpeed;

    public Transform bottomRightBoundary, topLeftBoundary;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xDir = Input.GetAxis("Horizontal");
        yDir = Input.GetAxis("Vertical");

        
        transform.position = new Vector3(transform.position.x + xDir * cameraMoveSpeed, transform.position.y + yDir * cameraMoveSpeed, transform.position.z);

        if(transform.position.x > bottomRightBoundary.position.x)
        {
            transform.position = new Vector3(bottomRightBoundary.position.x, transform.position.y, transform.position.z);
        }

        if (transform.position.x < topLeftBoundary.position.x)
        {
            transform.position = new Vector3(topLeftBoundary.position.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y > topLeftBoundary.position.y)
        {
            transform.position = new Vector3(transform.position.x, topLeftBoundary.position.y, transform.position.z);
        }

        if (transform.position.y < bottomRightBoundary.position.y)
        {
            transform.position = new Vector3(transform.position.x, bottomRightBoundary.position.y, transform.position.z);
        }
    }
}
