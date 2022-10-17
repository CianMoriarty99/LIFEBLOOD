using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCardToGrid : MonoBehaviour
{
    public CardController card;
    public Vector2 coords;
    // Start is called before the first frame update
    void Start()
    {
        card = null;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D currentCard = Physics2D.OverlapPoint(transform.position, 1, -1.45f, -1.55f);
        Collider2D hit = Physics2D.OverlapPoint(transform.position, 1, 0, 5);

        if (!currentCard && hit && hit.gameObject.CompareTag("Card"))
        {
            card = hit.gameObject.GetComponent<CardController>();
            card.mostRecentNode = this.transform;
            card.mostRecentNodeCoords = coords;
            Debug.Log(coords);
        }

        
    }

    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }
}
