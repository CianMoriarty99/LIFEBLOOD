using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCardController : MonoBehaviour
{
    // Start is called before the first frame update
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
        if (!haveShaken)
        {
            sh.Shake(0.3f, 0.2f);
            haveShaken = true;
        }
    }
}
