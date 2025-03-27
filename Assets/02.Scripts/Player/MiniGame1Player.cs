using UnityEngine;
using Lean.Touch;

namespace DocumentGame
{
    public class MiniGame1Player : MonoBehaviour
    {
        public GameObject Shredder;
        public GameObject Safe;
        public GameObject Origin;

        private Vector3 _direction;
        private bool _status;
        private Document _current;
        private Vector3 _shredder;
        private Vector3 _safe;

        private void OnEnable()
        {
            LeanTouch.OnFingerSwipe += HandleFingerSwipe;
            _shredder = Shredder.transform.position - Origin.transform.position;
            _safe = Safe.transform.position - Origin.transform.position;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerSwipe -= HandleFingerSwipe;
        }

        void Update()
        {
            if (!_status)
            {
                return;
            }

            if (_current == null)
            {
                _current = DocumentGameManager.Instance.GetDocument();
                return;
            }

            //GetKeycode();

            if (_direction == Vector3.zero)
            {
                return;
            }

            _current.Move(_direction);
            _current = null;
            _direction = Vector3.zero;
            DocumentGameManager.Instance.Process();
        }

        private void HandleFingerSwipe(LeanFinger finger)
        {
            Vector2 swipeDelta = finger.SwipeScreenDelta;

            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0)
                {
                    _direction = _shredder;
                }
                else
                {
                    _direction = _safe;
                }
            }
            else
            {
                if (swipeDelta.y < 0)
                {
                    _direction = Vector3.down;
                }
            }
        }

        private void GetKeycode()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _direction = Vector3.down;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _direction = Vector3.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _direction = Vector3.right;
            }
        }

        public void GameStart()
        {
            _current = null;
            _status = true;
            Shredder.SetActive(true);
            Safe.SetActive(true);
        }

        public void GameOver()
        {
            _status = false;
            Shredder.SetActive(false);
            Safe.SetActive(false);
        }
    }
}
