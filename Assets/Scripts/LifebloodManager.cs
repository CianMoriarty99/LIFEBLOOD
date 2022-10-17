using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifebloodManager : MonoBehaviour
{
    public static LifebloodManager instance;
    public enum PHASE
    {
        PLACEPHASE,
        BATTLEPHASE,
    }

    // Start is called before the first frame update
    public int lifeblood, maxLifeblood, playCostThisTurn;
    public TextMeshPro lifebloodText;

    public GameObject bloodBar;

    public float heightMultiplier;

    public Vector3 startingPosition;

    GameManager gm;

    public GameObject placePhaseGraphic, battlePhaseGraphic;

    public float distancePerLifeBloodStep = 37f;

    public PHASE phase;

    public bool mousingOver, battleEnded;

    public List<(CardController, float, SubPhasePlayed)> spellOrder = new List<(CardController, float, SubPhasePlayed)>();

    public Card[,] board  = new Card[5,2];

    public float[] boardSpaceInWorldX;
    public float[] boardSpaceInWorldY;

    void Start()
    {
        instance = this;
        heightMultiplier = 0;
        startingPosition = bloodBar.transform.position;
        gm = GameManager.instance;
        lifeblood = gm.lifeblood;
        maxLifeblood = gm.maxLifeblood;
        phase = PHASE.PLACEPHASE;
    }

    // Update is called once per frame
    void Update()
    {

        lifebloodText.text = (lifeblood - playCostThisTurn).ToString();

        bloodBar.transform.position = new Vector3(startingPosition.x - ((maxLifeblood - lifeblood) / distancePerLifeBloodStep), startingPosition.y, startingPosition.z);
        if (playCostThisTurn > 0)
        {
            lifebloodText.color = Color.red;
        } 
        else
        {
            lifebloodText.color = Color.white;
        }


        if(mousingOver && Input.GetMouseButtonDown(0) && phase == PHASE.PLACEPHASE)
        {
            StartCoroutine(TurnChanged());
        }
    }

    IEnumerator TurnChanged()
    {
        yield return new WaitForSeconds(1f);
        if (phase == PHASE.PLACEPHASE)
        {
            phase = PHASE.BATTLEPHASE;
            placePhaseGraphic.SetActive(false);
            battlePhaseGraphic.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            battlePhaseGraphic.SetActive(false);
            StartCoroutine(BattleSequence());
        } else 
        {
            battlePhaseGraphic.SetActive(false);
            placePhaseGraphic.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            placePhaseGraphic.SetActive(false);
            battleEnded = true;
            spellOrder = new List<(CardController, float, SubPhasePlayed)>();
            phase = PHASE.PLACEPHASE;
            lifeblood -= playCostThisTurn;
            playCostThisTurn = 0;
            yield return new WaitForSeconds(0.1f);
            battleEnded = false;
            
        }
    }

    IEnumerator BattleSequence()
    {
        
        foreach ((CardController cc, float f, SubPhasePlayed spp) in spellOrder)
        {
            if(spp == SubPhasePlayed.Prep)
            { 
                cc.castSpellNow = true;
                yield return new WaitForSeconds(f + 1f);
            }
        }
        yield return new WaitForSeconds(0.3f);

        foreach ((CardController cc, float f, SubPhasePlayed spp) in spellOrder)
        {
            if (spp == SubPhasePlayed.Battle)
            {
                cc.castSpellNow = true;
                yield return new WaitForSeconds(f + 1f);
            }
        }

        StartCoroutine(TurnChanged());
    }



    //The hitbox is the "Change Phase" button
    private void OnMouseEnter()
    {
        mousingOver = true;
    }

    private void OnMouseExit()
    {
        mousingOver = false;
    }
}
