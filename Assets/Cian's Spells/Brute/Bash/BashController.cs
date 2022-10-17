using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target1, target2;
    public float speed1, speed2;
    public GameObject[] gos;
    ScreenShake sh;
    public bool haveShaken;

    void Start()
    {
        sh = ScreenShake.instance;
        haveShaken = false;
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed1 * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target1.position, step);

        // Check if the position of this and target are approximately equal.
        if (Vector3.Distance(transform.position, target1.position) < 0.001f)
        {
            if(!haveShaken)
            {
                sh.Shake(0.3f, 0.2f);
                haveShaken = true;
            }

            foreach (GameObject go in gos)
            {
                go.SetActive(true);
            }

            target1 = target2;
            speed1 = speed2;
        }
 
    }
}
