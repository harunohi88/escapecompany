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
        private bool _fever;

        private void OnEnable()
        {
            LeanTouch.OnFingerSwipe += HandleFingerSwipe;
            LeanTouch.OnFingerTap += HandleFingerTap;
            _shredder = Shredder.transform.position - Origin.transform.position;
            _safe = Safe.transform.position - Origin.transform.position;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerSwipe -= HandleFingerSwipe;
            LeanTouch.OnFingerTap -= HandleFingerTap;
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

            if (_direction == Vector3.zero)
            {
                return;
            }

            _current.Move(_direction);
            _current = null;
            _direction = Vector3.zero;
            DocumentGameManager.Instance.Process();
        }

        private void HandleFingerTap(LeanFinger finger)
        {
            // 도망 버튼 등 UI 위에서 시작한 터치는 문서 분류로 치지 않는다
            if (finger.StartedOverGui) return;

            if (!_fever || _current == null)
            {
                return;
            }

            if (_current.Type == DocumentType.Important)
            {
                _direction = _safe;
            }
            else
            {
                _direction = _shredder;
            }
        }

        private void HandleFingerSwipe(LeanFinger finger)
        {
            if (finger.StartedOverGui) return;

            if (_current == null)
            {
                return;
            }

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
        }

        public void GameStart()
        {
            _current = null;
            _direction = Vector3.zero;
            _status = true;
            _fever = false;
            Shredder.SetActive(true);
            Safe.SetActive(true);
        }

        // 도망: 입력을 멈추고 파쇄기/금고를 숨긴다
        public void Suspend()
        {
            _current = null;
            _direction = Vector3.zero;
            _status = false;
            Shredder.SetActive(false);
            Safe.SetActive(false);
        }

        // 이어하기: 피버 상태도 그대로 복원
        public void Resume(bool fever)
        {
            _current = null;
            _direction = Vector3.zero;
            _status = true;
            _fever = fever;
            Shredder.SetActive(true);
            Safe.SetActive(true);
        }

        public void GameOver()
        {
            _current = null;
            _status = false;
            _fever = false;
            Shredder.SetActive(false);
            Safe.SetActive(false);
        }

        public void Fever()
        {
            _fever = true;
        }

        public void FeverEnd()
        {
            _fever = false;
        }
    }
}
