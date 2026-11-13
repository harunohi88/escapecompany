using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace CreateMap
{


    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        public GameObject DialogueBox;
        public TextMeshProUGUI DialogueText; // UI 텍스트 (대사 표시)
        public string[] Dialogues; // 대사 목록
        public int CurrentIndex = 0; // 현재 대사 인덱스

        public event Action OnDialogueEnded;

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Start()
        {
            NextDialogue();
        }

        // 튜토리얼이 끝났는지 (늦게 구독하는 쪽에서 확인용)
        public bool HasEnded { get; private set; }

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