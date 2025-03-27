using UnityEngine;
using DG.Tweening;


public class Document : MonoBehaviour
{
    public int DefaultScore;
    public Vector3 Direction;
    public float Duration;

    public void Move(Vector3 direction)
    {
        if (direction == Direction)
        {
            DocumentGameManager.Instance.AddScore(DefaultScore);
        }

        transform.DOMove(transform.position + direction, Duration);
    }
}
