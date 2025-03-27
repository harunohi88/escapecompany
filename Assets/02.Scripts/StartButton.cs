using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnClickGameStart()
    {
        DocumentGameManager.Instance.NewGame();
    }
}
