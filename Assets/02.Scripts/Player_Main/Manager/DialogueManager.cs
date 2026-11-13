using System;
using TMPro;
using UnityEngine;

namespace CreateMap
{
    // 튜토리얼 단계. 값 = 지금까지 표시한 대사 수(CurrentIndex)이며 Dialogues 배열 순서와 1:1로 맞물린다.
    // 대사를 추가/삭제하면 여기만 고치면 되도록, 다른 스크립트는 숫자 대신 이 이름으로 비교한다.
    public enum TutorialStep
    {
        NotStarted = 0,
        GoToComputer = 1,   // "캐릭터를 움직여 컴퓨터 앞으로 가자!"
        PlayMiniGame2 = 2,  // "노란 버튼을 눌러 미니게임을 플레이!" (컴퓨터)
        GoToBox = 3,        // "잘했어!! 다음은 왼쪽 아래 박스앞으로 가자!"
        PlayMiniGame1 = 4,  // "노란 버튼을 눌러 미니게임을 플레이!" (문서)
        StunBoss = 5,       // "도망다닐 때 붉은 버튼을 누르면 부장님을 기절 시킬 수 있어!"
        Survive = 6,        // "이제 6시까지 버텨봐!"
    }

    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        public GameObject DialogueBox;
        public TextMeshProUGUI DialogueText; // UI 텍스트 (대사 표시)
        public string[] Dialogues; // 대사 목록
        public int CurrentIndex = 0; // 현재 대사 인덱스

        public event Action OnDialogueEnded;

        // 튜토리얼이 끝났는지 (늦게 구독하는 쪽에서 확인용)
        public bool HasEnded { get; private set; }

        public TutorialStep Step => (TutorialStep)CurrentIndex;

        // 튜토리얼이 있는 씬에서 지금 해당 단계인지. 튜토리얼이 없는 씬(리플레이)에서는 항상 false
        public static bool IsStep(TutorialStep step)
        {
            return Instance != null && Instance.Step == step;
        }

        void Awake()
        {
            Instance = this;

            int expected = (int)TutorialStep.Survive;
            if (Dialogues.Length != expected)
            {
                Debug.LogWarning($"DialogueManager: 대사 수({Dialogues.Length})가 TutorialStep 단계 수({expected})와 다릅니다. TutorialStep을 함께 수정하세요.");
            }
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Start()
        {
            NextDialogue();
        }

        void EndDialogue()
        {
            DialogueBox.SetActive(false);
            if (HasEnded) return; // 종료 이벤트는 한 번만
            HasEnded = true;
            OnDialogueEnded?.Invoke();
        }


        public void ActiveDialogueBox()
        {
            DialogueBox.SetActive(true);
        }
        public void DeActiveDialogueBox()
        {
            DialogueBox.SetActive(false);
        }
        public void NextDialogue()
        {
            if (CurrentIndex < Dialogues.Length)
            {
                DialogueText.text = Dialogues[CurrentIndex]; // 대사 업데이트
                CurrentIndex++;
            }
            else
            {
                // 보스 이동시작, 미니게임 게이지, 타이머
                EndDialogue();
            }
        }

        public void ActiveButton()
        {
            EndDialogue();
        }

    }
}
