using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MarkerType {
    BATTLE,
    REST,
    QUEST
}

public class MapMarker : MonoBehaviour
{
    public MarkerType markerType;
    public Sprite disabled, available, traveled, playerIsHere;
    public int depthIndex;

    public bool nodeIsEnabled = false;

    SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = disabled;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentIndex == (depthIndex - 1))
        {
            nodeIsEnabled = true;
            spr.sprite = available;
        } 
        else 
        {
            nodeIsEnabled = false;
            return;
        }


    }

    void PlayerArrives()
    {
        spr.sprite = playerIsHere;
        GameManager.instance.currentIndex += 1;
        transform.GetChild(0).gameObject.SetActive(true);

        switch (markerType)
        {
            case MarkerType.BATTLE:
                GameManager.instance.StartNewBattle();
                break;
            case MarkerType.REST:
                GameManager.instance.ShowRestToast();
                break;
            case MarkerType.QUEST:
                GameManager.instance.ShowQuestToast();
                break;
        }
    }

    void PlayerLeaves()
    {
        spr.sprite = traveled;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if(nodeIsEnabled)
        {
            PlayerArrives();
        }
    }
}
