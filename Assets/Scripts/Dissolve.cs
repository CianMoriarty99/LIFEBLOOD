using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public float fade, timeElapsed;
    Material dissolve;
    SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        dissolve = m_SpriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        DissolveR();
    }

    void DissolveR()
    {

        fade = 1 - (Mathf.Sin(timeElapsed / 10) + 1) /2 ;
        dissolve.SetFloat("_Fade", fade);

    }
}
