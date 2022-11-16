using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyCardController : MonoBehaviour
{
    public Card card;
    SpriteRenderer m_SpriteRenderer;
    private int currentHealth;
    public TextMeshPro health, energyCost;
    public bool dragging, mousingOver, firstTimeBeingPlayed, isDissolving;
    public Transform mostRecentNode;
    public DeckController deck;
    public GameObject defaultBigCard, worldViewDetails;
    public Vector3 startingScale;
    public LifebloodManager lbm;
    public Material defaultMaterial, castMaterial, hoverMaterial, destroyMaterial;
    public float dissolveTime;
    public GameObject destroyCardAnimation;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = m_SpriteRenderer.material;
        castMaterial = card.castMaterial;
        hoverMaterial = card.hoverMaterial;
        currentHealth = card.maxHealth;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = card.artwork[0];
        health.text = currentHealth.ToString();
        energyCost.text = card.energyCost.ToString();
        dragging = false;
        mousingOver = false;
        firstTimeBeingPlayed = true;
        startingScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        health.sortingOrder = 1 + m_SpriteRenderer.sortingOrder;
        energyCost.sortingOrder = 1 + m_SpriteRenderer.sortingOrder;

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

        if(isDissolving)
        {
            m_SpriteRenderer.material.SetFloat("_DissolveAmount", dissolveTime);
            dissolveTime += Time.deltaTime;
        }    
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            StartCoroutine(DestroyCardAnimation());
        }
        else
        {
            StartCoroutine(TakeDamageAnimation());
        }
    }

    #region Animations

    IEnumerator DestroyCardAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        health.text = "";
        var anim = Instantiate(destroyCardAnimation, this.transform);
        m_SpriteRenderer.material = destroyMaterial;
        isDissolving = true;
        dissolveTime = 0;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        Destroy(anim);
    }

    IEnumerator TakeDamageAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        health.text = currentHealth.ToString(); //Add this to the animation part so it gets delayed correctly.
        
    }
    #endregion
    #region Events
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
    #endregion
}
