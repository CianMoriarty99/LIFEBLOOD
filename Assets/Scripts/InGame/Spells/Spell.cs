using UnityEngine;

[System.Serializable]
public class Spell
{
    public string desc;
    public SubPhasePlayed subPhase;
    public Position[] affectedTiles;
    public GameObject animation;
    public float animationTime;
    public bool isGlobal;
}