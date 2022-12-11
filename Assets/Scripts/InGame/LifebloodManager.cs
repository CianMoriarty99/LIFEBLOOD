using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public float heightMultiplier;

    public Vector3 startingPosition;

    GameManager gm;

    public GameObject placePhaseGraphic, battlePhaseGraphic;

    public float distancePerLifeBloodStep = 37f;

    public PHASE phase;

    public bool mousingOver, battleEnded, hasBattled;

    public List<(CardController, float, SubPhasePlayed)> spellOrder = new List<(CardController, float, SubPhasePlayed)>();

    public CardController[,] board = new CardController[(int)Board.Width, (int)Board.Height];


    public float[] boardSpaceInWorldX;
    public float[] boardSpaceInWorldY;

    void Start()
    {
        instance = this;
        heightMultiplier = 0;
        gm = GameManager.instance;
        lifeblood = gm.lifeblood;
        maxLifeblood = gm.maxLifeblood;
        phase = PHASE.PLACEPHASE;
    }

    // Update is called once per frame
    void Update()
    {

        lifebloodText.text = (lifeblood - playCostThisTurn).ToString();

        if (playCostThisTurn > 0)
        {
            lifebloodText.color = Color.red;
        }
        else
        {
            lifebloodText.color = Color.white;
        }


        if (mousingOver && Input.GetMouseButtonDown(0) && phase == PHASE.PLACEPHASE)
        {
            StartCoroutine(TurnChanged());
            hasBattled = true;
        }

        CheckForBattleEnd();


    }

    bool CheckForAliveCard(CardController[,] board, bool checkPlayer)
    {

        if(checkPlayer)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (board[i, j] != null)
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    if (board[i, j] != null)
                    {
                        return false;
                    }
                }
            }
        }


        return true;
    }

    void CheckForBattleEnd()
    {
        if (hasBattled)
        {
            bool playerLost = CheckForAliveCard(board, true);
            bool enemyLost = CheckForAliveCard(board, false);

            if (playerLost)
            {
                StartCoroutine(LoseGameSequence());
            } 
            else
            if (enemyLost)
            {
                StartCoroutine(WinBattleSequence());
            }

            
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
        }
        else
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
            if (spp == SubPhasePlayed.Prep)
            {
                CastSpell(cc);
                yield return new WaitForSeconds(f + 1f);
            }
        }
        yield return new WaitForSeconds(0.3f);

        foreach ((CardController cc, float f, SubPhasePlayed spp) in spellOrder)
        {
            if (spp == SubPhasePlayed.Battle)
            {
                CastSpell(cc);
                yield return new WaitForSeconds(f + 1f);
            }
        }

        StartCoroutine(TurnChanged());
    }

    #region Battle phase
    private void CastSpell(CardController cc)
    {
        var spell = cc.GetCurrentSpell();
        StartCoroutine(cc.CastSpell());


        var spellCardPosition = cc.boardPosition;
        var affectedTiles = spell.affectedTiles
            .Select(tile => spellCardPosition.Add(tile, cc.isEnemyCard))
            .Where(tile => tile.IsValid());


        //Global spells
        if(spell.affectedTiles.Length == 0)
        {
            foreach (var card in board)
            {
                if(card && card.isEnemyCard != cc.isEnemyCard)
                    card.TakeDamage(spell.damage);
            }
        } else
        {
            //Local spells
            foreach (var cardPosition in affectedTiles)
            {
                var enemyCard = board[(int)cardPosition.x, (int)cardPosition.y];
                if (enemyCard && enemyCard.isEnemyCard != cc.isEnemyCard)
                {
                    enemyCard.TakeDamage(spell.damage);
                }
            }
            }

    }
    #endregion

    IEnumerator LoseGameSequence()
    {
        hasBattled = false;
        yield return new WaitForSeconds(2f);
    }

    IEnumerator WinBattleSequence()
    {
        hasBattled = false;
        yield return new WaitForSeconds(2f);
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
