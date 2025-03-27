using UnityEngine;


namespace DocumentGame
{
    public class StartButton : MonoBehaviour
    {
        public void OnClickGameStart()
        {
            DocumentGameManager.Instance.NewGame();
        }
    }
}
