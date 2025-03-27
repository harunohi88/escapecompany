using UnityEngine;
using DG.Tweening;

namespace DocumentGame
{
    public class Document : MonoBehaviour
    {
        public int DefaultScore;
        public Vector3 Direction;
        public float Duration;

        public void Move(Vector3 direction)
        {
            if (direction == Direction)
            {
                DocumentGameManager.Instance.Correct(DefaultScore);
            }
            else
            {
                DocumentGameManager.Instance.Wrong(DefaultScore);
            }

            transform.DOMove(transform.position + direction * 5f, Duration).OnComplete(() => Destroy(gameObject));
        }
    }
}
