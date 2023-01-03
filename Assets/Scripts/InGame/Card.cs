using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
    public Sprite[] artwork;
    public Sprite backArtwork;
    public CardName cardName;
    public int energyCost;
    public int maxHealth;
    public Spell[] spells;
    public GameObject passiveAnimation;
    public Material castMaterial;
    public Material hoverMaterial;
    public int placementPriority;
    public GameObject aura;

}

public enum CardName
{
    BRUISER,
    PRIESTESS,
}
