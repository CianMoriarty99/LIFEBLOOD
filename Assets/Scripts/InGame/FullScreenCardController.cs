using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullScreenCardController : MonoBehaviour
{
    public Transform startingPositionInGame;
    public Card card;
    public TextMeshPro cardName, health, energyCost, damage, passiveDesc, e1Desc, e2Desc, e3Desc;
    SpriteRenderer spriteR;
    GameManager gm;
    // Start is called before the first frame update
    void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        
    }
    private void Start()
    {
        gm = GameManager.instance;
    }

    void Update()
    {
        if (card)
        {
            if (gm.currentScene == "InGame")
            {
                this.transform.position = startingPositionInGame.position;
                
            }
            if (gm.currentScene == "WorldView")
            {
                this.transform.position = this.transform.parent.position;
            }
            spriteR.sprite = card.artwork[0];
            cardName.text = card.cardName;
            //health.text = "Health: " + card.health.ToString();
            //energyCost.text = "Cost: " + card.energyCost.ToString();
            //damage.text = "Damage: " + card.baseDamage.ToString();
            //if(card.spells.Length > 3)
            //{
            //    passiveDesc.text = card.spells[0].desc;
            //    e1Desc.text = card.spells[1].desc;
            //    e2Desc.text = card.spells[2].desc;
            //    e3Desc.text = card.spells[3].desc;
            //}

        }
    }

    private void LateUpdate()
    {
        if (gm.currentScene == "WorldView")
            this.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        else
            this.transform.localScale = new Vector3(2f, 2f, 2f);
    }
}