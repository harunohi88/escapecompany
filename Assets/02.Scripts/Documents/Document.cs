using UnityEngine;
using DG.Tweening;

public enum DocumentType
{
    Important,
    Trash
}

namespace DocumentGame
{
    public class Document : MonoBehaviour
    {
        public DocumentType Type;
        public int DefaultScore;
        public Vector3 Direction;
        public float Duration;

        public void Move(Vector3 direction)
        {
            if (Direction.x * direction.x > 0)
            {
                DocumentGameManager.Instance.Correct(DefaultScore);
            }
            else
            {
                DocumentGameManager.Instance.Wrong(DefaultScore, Type);
            }

            transform.DOMove(transform.position + direction, Duration).OnComplete(() => Destroy(gameObject));
        }
    }
}
