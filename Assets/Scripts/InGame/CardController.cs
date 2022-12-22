using TMPro;
using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
    public Card card;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField]
    private TextMeshPro health, energyCost;
    private bool dragging, mousingOver, firstTimeBeingPlayed, isDissolving, hasntAddedSpellsYet, castingSpell;
    public bool isEnemyCard;
    public Transform mostRecentNode;
    public DeckController deck;
    public GameObject defaultBigCard, worldViewDetails;
    public Vector3 startingScale;
    public LifebloodManager lbm;
    GameManager gm;
    ScreenShake sh;
    public int powerLevel, damageReduction;
    private int currentHealth;
    public GameObject spawnAnimationPrefab, spellCastAnimation, passiveAnimationPrefab, passiveAnimation, destroyCardAnimation;
    public Position boardPosition, mostRecentNodeCoords;
    public Material defaultMaterial, castMaterial, hoverMaterial, destroyMaterial;
    public float fadeTimer, defaultTimer, dissolveTime;
    public GameObject aura;

    //Auras
    public bool reductionAuraApplied;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        EnsureRenderTextOverCardAlways();
        FindExternalComponents();
        CheckForDropCard();
        CardBackDescriptionDisplay();
        ResetPowerLevelOnTurnEnd();
        IncreaseCardPowerLevel();
        AddCardToBattleOrder();
        PlayPassiveEffect();
        ChangeMaterial();
        UpdateDamageReduction();
        InitialiseAura();
    }

    void InitialiseAura()
    {
        if(aura)
        {
            aura.SetActive(false);
        }

        if (card.cardName == CardName.BRUISER)
        {
            if(powerLevel == 2)
            {
                aura.SetActive(true);

                if(isEnemyCard)
                {
                    aura.tag = "BRUISER_DAMAGE_REDUCTION_ENEMY";
                }
                else
                {
                    aura.tag = "BRUISER_DAMAGE_REDUCTION_ALLY";
                }
                
            }
        }

    }

    void UpdateDamageReduction()
    {
        int tmp = 0;

        //BRUISER
        if(card.cardName == CardName.BRUISER)
        {
            tmp += (powerLevel - 1);
        }

        //REDUCTION AURA
        if(reductionAuraApplied)
        {
            tmp += 2;
        }

        damageReduction = tmp;
        
    }

    void Init()
    {
        isEnemyCard = false;
        currentHealth = card.maxHealth;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = card.artwork[0];
        defaultMaterial = m_SpriteRenderer.material;
        castMaterial = card.castMaterial;
        hoverMaterial = card.hoverMaterial;
        health.text = currentHealth.ToString();
        energyCost.text = card.energyCost.ToString();
        dragging = false;
        mousingOver = false;
        firstTimeBeingPlayed = true;
        startingScale = this.transform.localScale;
        hasntAddedSpellsYet = true;
        passiveAnimationPrefab = card.passiveAnimation;
        castingSpell = false;
        fadeTimer = 0.5f;
        defaultTimer = 0.5f;
    }

    void AddCardToBattleOrder()
    {
        if (lbm && lbm.phase == LifebloodManager.PHASE.BATTLEPHASE && hasntAddedSpellsYet && powerLevel != 0)
        {
            Spell c = card.spells[powerLevel];
            lbm.spellOrder.Add((this, c.animationTime, c.subPhase));
            hasntAddedSpellsYet = false;
        }

        if (!hasntAddedSpellsYet && lbm.phase == LifebloodManager.PHASE.PLACEPHASE)
        {
            hasntAddedSpellsYet = true;
        }
    }

    void IncreaseCardPowerLevel()
    {
        if (!isEnemyCard && lbm && lbm.phase == LifebloodManager.PHASE.PLACEPHASE && Input.GetMouseButtonDown(1) && mousingOver)
        {
            if (powerLevel == 3)
            {
                powerLevel = 0;
                lbm.playCostThisTurn -= 3;
            }
            else
            {
                lbm.playCostThisTurn++;
                powerLevel++;
            }

            m_SpriteRenderer.sprite = card.artwork[powerLevel];

        }
    }

    void PlayPassiveEffect()
    {
        if (mousingOver && !firstTimeBeingPlayed && !dragging)
        {
            if (fadeTimer + 0.1f <= 0)
            {
                Vector3 locationOfSpellInstance = new Vector3(transform.position.x, transform.position.y, -1.5f);

                if (passiveAnimation != null)
                {
                    passiveAnimation.SetActive(true);
                    passiveAnimation.transform.position = locationOfSpellInstance;
                }
                else
                {
                    passiveAnimation = Instantiate(passiveAnimationPrefab, locationOfSpellInstance, Quaternion.identity);
                }
            }
        }
        else
        {
            if (passiveAnimation != null)
            {
                passiveAnimation.SetActive(false);
            }
        }
    }

    void ChangeMaterial()
    {
        if (castingSpell)
        {
            m_SpriteRenderer.material = castMaterial;
            fadeTimer = defaultTimer;
        }
        else if (mousingOver && !firstTimeBeingPlayed && !dragging)
        {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer <= 0)
            {
                m_SpriteRenderer.material = hoverMaterial;
            }

        }
        else if (isDissolving)
        {
            m_SpriteRenderer.material.SetFloat("_DissolveAmount", dissolveTime);
            dissolveTime += Time.deltaTime;
        }
        else
        {
            fadeTimer = defaultTimer;
            m_SpriteRenderer.material = defaultMaterial;
        }

    }

    void ResetPowerLevelOnTurnEnd()
    {
        if (lbm && lbm.battleEnded == true)
        {
            powerLevel = 0;
            m_SpriteRenderer.sprite = card.artwork[powerLevel];
        }
    }

    void CardBackDescriptionDisplay()
    {
        if (mousingOver && !dragging)
        {
            defaultBigCard.SetActive(true);
            defaultBigCard.GetComponent<FullScreenCardController>().card = card;

            if (gm.currentScene == Scene.WorldView)
            {
                m_SpriteRenderer.enabled = false;
                health.sortingOrder = this.m_SpriteRenderer.sortingOrder - 1;
                energyCost.sortingOrder = this.m_SpriteRenderer.sortingOrder - 1;
            }
        }
        else
        {
            m_SpriteRenderer.enabled = true;
            defaultBigCard.SetActive(false);
        }
    }

    void CheckForDropCard()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                DropCard();
            }

        }
    }


    void FindExternalComponents()
    {
        if (!gm)
        {
            gm = GameManager.instance;
        }

        if (!deck)
        {
            if (gm && gm.currentScene == Scene.InGame)
            {
                deck = DeckController.instance;
            }
        }

        if (!lbm)
        {
            if (gm && gm.currentScene == Scene.InGame)
            {
                lbm = LifebloodManager.instance;
            }
        }

        if (!sh)
        {
            sh = ScreenShake.instance;
        }
    }

    void EnsureRenderTextOverCardAlways()
    {
        health.sortingOrder = 1 + this.m_SpriteRenderer.sortingOrder;
        energyCost.sortingOrder = 1 + this.m_SpriteRenderer.sortingOrder;
    }

    void DropCard()
    {
        this.transform.localScale = new Vector3(1.15f * startingScale.x, 1.15f * startingScale.y, 1f);
        m_SpriteRenderer.sortingOrder = 0;

        if (mostRecentNode != null)
        {
            this.transform.position = new Vector3(mostRecentNode.position.x, mostRecentNode.position.y, mostRecentNode.position.z - 0.1f);
            boardPosition = mostRecentNodeCoords;
            lbm.board[boardPosition.x, boardPosition.y] = this;
            this.m_SpriteRenderer.sortingOrder = -7;
            var tmp = GetComponent<EnableHighlight>().highlight.GetComponent<SpriteRenderer>().sortingOrder = -8;

            if (firstTimeBeingPlayed)
            {
                lbm.playCostThisTurn += card.energyCost;
                firstTimeBeingPlayed = false;
                StartCoroutine(PlaySpawnAnimation());
            }

        }
        else
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, -1.65f);
        }

        dragging = false;
    }



    public void TakeDamage(int damage)
    {
        
        int dmg = Mathf.Clamp((damage - damageReduction), 0, damage);

        currentHealth -= dmg;

        health.text = currentHealth.ToString();

        if (currentHealth <= 0)
        {
            StartCoroutine(DestroyCardAnimation());
        } else
        {
            StartCoroutine(TakeDamageAnimation());
        }
    }

    #region Animations
    IEnumerator PlaySpawnAnimation()
    {
        var obj = Instantiate(spawnAnimationPrefab, transform.position, spawnAnimationPrefab.transform.rotation);
        obj.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        obj.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }

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

    #region Spells
    public IEnumerator CastSpell()
    {
        var spell = card.spells[powerLevel];
        castingSpell = true;

        float a = lbm.boardSpaceInWorldX[boardPosition.x];
        float b = lbm.boardSpaceInWorldY[boardPosition.y];

        Vector3 locationOfSpellInstance = new Vector3(a, b, -1.5f);

        int r = isEnemyCard ? 180 : 0;

        Vector3 rot = spell.animation.transform.rotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(rot.x + r, rot.y, rot.z);

        var spellAnimation = Instantiate(spell.animation, locationOfSpellInstance, rotation);

        ParticleSystem[] ps = spellAnimation.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem p in ps)
        {
            p.Play();
        }

        yield return new WaitForSeconds(spell.animationTime);

        foreach (ParticleSystem p in ps)
        {
            p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        Destroy(spellAnimation);

        castingSpell = false;
        yield return new WaitForSeconds(0.5f);
    }

    public Spell GetCurrentSpell()
    {
        return card.spells[powerLevel];
    }
    #endregion
    #region Events
    void OnMouseDrag()
    {
        if (!isEnemyCard && lbm && lbm.phase == LifebloodManager.PHASE.PLACEPHASE)
        {
            if (gm.currentScene == Scene.InGame)
            {
                dragging = true;
                deck.isOpen = false;
                this.transform.SetParent(null);
                m_SpriteRenderer.sortingOrder = 100;
                this.transform.localScale = new Vector3(1.25f * startingScale.x, 1.25f * startingScale.y, 1f);
                float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
                transform.position = new Vector3(pos_move.x, pos_move.y, 2f);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (lbm && lbm.phase == LifebloodManager.PHASE.PLACEPHASE)
        {
            mousingOver = true;
            this.transform.localScale = new Vector3(1.25f * startingScale.x, 1.25f * startingScale.y, 1f);
        }
    }

    private void OnMouseExit()
    {
        mousingOver = false;
        this.transform.localScale = startingScale;
    }


    #endregion
}
