using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;
    public Transform closedState, openState;
    public bool isOpen, mousingOver;
    public GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        this.transform.position = closedState.position;
        isOpen = false;
        mousingOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mousingOver)
        {
            isOpen = !isOpen;
        }

        if (isOpen)
        {
            this.transform.position = openState.position;
        } 
        else
        {
            this.transform.position = closedState.position;
        }
               
        
    }

    private void OnMouseOver()
    {
        highlight.SetActive(true);
        mousingOver = true;
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
        mousingOver = false;
    }

}
