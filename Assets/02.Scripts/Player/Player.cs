using UnityEngine;

public class Player : MonoBehaviour
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
            _current = DocumentGameManager.Instance.AskDocument();
        }

        GetKeycode();
        if (_direction == Vector3.zero)
        {
            return;
        }

        _current.Move(_direction);
        _current = DocumentGameManager.Instance.AskDocument();
        _direction = Vector3.zero;
    }

    private void GetKeycode()
    {
        Debug.Log("GetKeycode");
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S");
            _direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
            _direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D");
            _direction = Vector3.right;
        }
    }

    public void GameStart()
    {
        Debug.Log("GameStart");
        _current = null;
        _status = true;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        _status = false;
    }
}
