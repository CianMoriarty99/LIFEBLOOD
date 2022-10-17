using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Monster,
    Spell
}

public enum CardRange
{
    Melee,
    Ranged
}

public enum SubPhasePlayed
{
    Prep,
    Battle
}

[System.Serializable]
public class Spell
{
    public string desc;
    public SubPhasePlayed subPhase;
    public Vector2[] effectedTiles;
    public GameObject animation;
    public float animationTime;
    public bool isGlobal;
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
    public Sprite[] artwork;
    public Sprite backArtwork;
    public string cardName;
    public CardType cardType;
    public CardRange cardRange;
    public int energyCost;
    public int health;
    public int baseDamage;
    public Spell[] spells;
    public GameObject passiveAnimation;
    public Material castMaterial; 
    public Material hoverMaterial;

}
