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

            // 대사창을 숨기고 '부장 기절' 안내를 미리 넣어 둔다. 미니게임1이 끝나면 DocumentGameManager가 보여 준다.
            if (!DialogueManager.IsStep(TutorialStep.PlayMiniGame1)) return;

            DialogueManager.Instance.DeActiveDialogueBox();
            DialogueManager.Instance.NextDialogue();
        }

        public void OnClickMiniGame2Start()
        {
            if (MiniGame2 == false) return;
            
            EnterMiniGameTwo();

            // 미니게임2 중에는 대사창만 숨긴다. 다음 대사로 넘기는 건 GameTwoManager.GameOver에서 한다.
            if (!DialogueManager.IsStep(TutorialStep.PlayMiniGame2)) return;

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
