using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHighlight : MonoBehaviour
{
    public GameObject highlight;
    bool dragging;


    private void Update()
    {
        dragging = false;
    }
    void OnMouseOver()
    {
        if (!dragging)
            highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    void OnMouseDrag()
    {
        dragging = true;
    }

   
}
