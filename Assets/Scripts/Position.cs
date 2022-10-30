using UnityEngine;

public class Position
{
    public int x { get; set; }
    public int y { get; set; }

    public Position(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
    }

    public Position(Vector2 vector)
    {
        x = (int)vector.x;
        y = (int)vector.y;
    }

}
