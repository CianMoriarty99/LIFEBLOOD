using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
    public Sprite[] artwork;
    public Sprite backArtwork;
    public string cardName;
    public int energyCost;
    public int currentHealth;
    public int maxHealth;
    public Spell[] spells;
    public GameObject passiveAnimation;
    public Material castMaterial;
    public Material hoverMaterial;
}
