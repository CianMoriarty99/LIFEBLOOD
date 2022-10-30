using UnityEngine;

[System.Serializable]
public class Position
{
    public int x;
    public int y;

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

    /// <summary>
    /// Check if the current position refers to a valid board location.
    /// </summary>
    public bool IsValid()
    {
        return x >= 0 && x < (int)Board.Width && y >= 0 && y < (int)Board.Height * 2;
    }

    /// <summary>
    /// Adds a position to the current one in either the direction that the enemy or allied cards are facing based on negativeY.
    /// </summary>
    /// <param name="newPosition"></param>
    public Position Add(Position newPosition, bool negativeY)
    {
        int yMultiplier = negativeY ? -1 : 1;
        return new Position(x + newPosition.x, y + newPosition.y * yMultiplier);
    }

}
