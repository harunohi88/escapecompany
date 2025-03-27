using UnityEngine;

namespace DocumentGame
{
    public class StartButton : MonoBehaviour
    {
        public GameObject miniGame2Root;


        public bool MiniGame1;
        public bool MiniGame2;
        public void OnClickMiniGame1Start()
        {
            if (MiniGame1 == false) return;
            DocumentGameManager.Instance.NewGame();
        }

        public void OnClickMiniGame2Start()
        {
            if (MiniGame2 == false) return;

            EnterMiniGameTwo();
        }

        void EnterMiniGameTwo()
        {
            miniGame2Root.SetActive(true);
        }

        public bool IsAnyGame()
        {
            return MiniGame1 || MiniGame2;
        }
    }
}
