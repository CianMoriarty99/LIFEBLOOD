using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyCardController : MonoBehaviour
{
    public Card card;
    SpriteRenderer m_SpriteRenderer;
    public TextMeshPro health, energyCost, damage;
    public bool dragging, mousingOver, firstTimeBeingPlayed, isDissolving;
    public Transform mostRecentNode;

    public DeckController deck;

    public GameObject defaultBigCard, worldViewDetails;

    public Vector3 startingScale;

    public LifebloodManager lbm;

    GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = card.artwork[0];
        health.text = card.health.ToString();
        energyCost.text = card.energyCost.ToString();
        damage.text = card.baseDamage.ToString();
        dragging = false;
        mousingOver = false;
        firstTimeBeingPlayed = true;
        startingScale = this.transform.localScale;
        isDissolving = false;



    }

    // Update is called once per frame
    void Update()
    {
        health.sortingOrder = 1 + m_SpriteRenderer.sortingOrder;
        energyCost.sortingOrder = 1 + m_SpriteRenderer.sortingOrder;
        damage.sortingOrder = 1 + m_SpriteRenderer.sortingOrder;


        if (mousingOver && !dragging)
        {
            defaultBigCard.SetActive(true);
            defaultBigCard.GetComponent<FullScreenCardController>().card = card;

        }
        else
        {
            m_SpriteRenderer.enabled = true;
            defaultBigCard.SetActive(false);
        }



    }
    private void OnMouseEnter()
    {
        mousingOver = true;
        this.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
    }

    private void OnMouseExit()
    {
        mousingOver = false;
        this.transform.localScale = startingScale;
    }

}