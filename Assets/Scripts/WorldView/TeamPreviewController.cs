using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPreviewController : MonoBehaviour
{
    GameManager gm;
    public List<GameObject> cards;
    public GameObject cardPrefab;
    public float zPosition = 0;

    void Start()
    {
        gm = GameManager.instance;
        PreviewCards();
    }


    // Update is called once per frame
    void Update()
    {
    }

    void PreviewCards()
    {
        List<Card> cardsToShow = gm.deck;
        int numberOfCards = cardsToShow.Count;

        for (int i=0; i<numberOfCards; i++)
        {
            float gap = 2.2f;
            Vector3 startingPosition = new Vector3(transform.position.x - (gap * (numberOfCards - 1) / 2) + gap * i, transform.position.y, zPosition) ;
            GameObject c = Instantiate(cardPrefab, startingPosition, Quaternion.identity);
            c.GetComponent<SpriteRenderer>().sortingOrder = 0;
            CardController cc = c.GetComponent<CardController>();
            cc.card = cardsToShow[i];
            c.transform.SetParent(this.transform);
            cards.Add(c);
        }
    }
}
