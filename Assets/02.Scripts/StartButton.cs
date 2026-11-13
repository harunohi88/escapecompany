using MiniGameTWo;
using UnityEngine;
using CreateMap;
namespace DocumentGame
{
    public class StartButton : MonoBehaviour
    {
        public GameObject miniGame2Root;
        public GameTwoManager miniGame2Manager;

        public bool MiniGame1;
        public bool MiniGame2;
        public void OnClickMiniGame1Start()
        {
            if (MiniGame1 == false) return;
            DocumentGameManager.Instance.NewGame();

            if (DialogueManager.Instance == null) return;
            if (DialogueManager.Instance.CurrentIndex != 4) return;

            DialogueManager.Instance.DeActiveDialogueBox();
            DialogueManager.Instance.NextDialogue();
        }

        public void OnClickMiniGame2Start()
        {
            if (MiniGame2 == false) return;
            
            EnterMiniGameTwo();

            if (DialogueManager.Instance == null) return;
            // 컴퓨터 앞 트리거에서 이미 대사가 넘어가 CurrentIndex는 2.
            // (기존 1 비교는 항상 실패해서 미니게임2 중에도 대사창이 떠 있었음)
            // 다음 대사로 넘기는 건 GameTwoManager.GameOver(CurrentIndex == 2)에서 한다.
            if (DialogueManager.Instance.CurrentIndex != 2) return;

            DialogueManager.Instance.DeActiveDialogueBox();
        }

        void EnterMiniGameTwo()
        {
            miniGame2Root.SetActive(true);
            miniGame2Manager.InitGame();
        }

        public bool IsAnyGame()
        {
            return MiniGame1 || MiniGame2;
        }
    }
}
