using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashController : MonoBehaviour
{
    ScreenShake sh;
    public bool haveShaken;

    // Start is called before the first frame update
    void Start()
    {
        sh = ScreenShake.instance;
        haveShaken = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!haveShaken)
        {
            sh.Shake(0.4f, 0.4f);
            haveShaken = true;
        }
    }
}
