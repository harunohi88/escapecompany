using UnityEngine;

public enum Direction
{
    Left,
    Right,
    Down
}

public class Document : MonoBehaviour
{
    public Direction Direction;
    public int Score;

    public void Move(Direction direction)
    {
        if (direction == Direction)
        {
            DocumentManager.Instance.AddScore(Score);
        }
    }
}
