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
            if (DialogueManager.Instance.CurrentIndex != 1) return;
            
            DialogueManager.Instance.DeActiveDialogueBox();
            DialogueManager.Instance.NextDialogue();
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
