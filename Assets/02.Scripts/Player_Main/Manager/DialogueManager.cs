using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        NextDialogue();
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
            DialogueBox.SetActive(false);
            OnDialogueEnded?.Invoke();
            // 보스 이동시작
            // 미니게임 게이지
            // 타이머

        }
    }
    
}