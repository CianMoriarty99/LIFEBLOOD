using UnityEngine;

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