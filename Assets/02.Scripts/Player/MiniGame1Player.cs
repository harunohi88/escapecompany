using UnityEngine;

namespace DocumentGame
{
    public class MiniGame1Player : MonoBehaviour
    {
        private Vector3 _direction;
        private bool _status;
        private Document _current;

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

            GetKeycode();

            if (_direction == Vector3.zero)
            {
                return;
            }

            _current.Move(_direction);
            _current = null;
            _direction = Vector3.zero;
            DocumentGameManager.Instance.Process();
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
        }

        public void GameOver()
        {
            _status = false;
        }
    }
}
