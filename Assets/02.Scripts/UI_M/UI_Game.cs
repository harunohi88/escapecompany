using DocumentGame;
using TMPro;
using UnityEngine;

namespace CreateMap
{
    public class UI_Game : MonoBehaviour
    {
        public static readonly int SECONDS_PER_MINUTE = 8;
        public TextMeshProUGUI GlobalTimer;

        float _second;
        bool pause;
        bool _started; // 튜토리얼이 끝나야 시계가 흐름

        public int Hour { get; private set; } = 5;
        public int Minute { get; private set; } = 50;
        void Start()
        {
            UpdateTimerText();

            // 구독은 한 번만 (기존엔 결과창이 뜰 때마다 ResumeTimer를 다시 등록해 누적됐음)
            UI_MiniGame1.Instance.ShowResultAction += PauseTimer;
            UI_MiniGame1.Instance.OnCloseButtonClicked += ResumeTimer;

            DialogueManager dialogue = DialogueManager.Instance;
            if (dialogue != null && !dialogue.HasEnded)
                dialogue.OnDialogueEnded += StartTimer;
            else
                StartTimer();
        }

        void OnDestroy()
        {
            if (UI_MiniGame1.Instance != null)
            {
                UI_MiniGame1.Instance.ShowResultAction -= PauseTimer;
                UI_MiniGame1.Instance.OnCloseButtonClicked -= ResumeTimer;
            }
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.OnDialogueEnded -= StartTimer;
        }

        void StartTimer()
        {
            _started = true;
        }

        void Update()
        {
            UpdateTimer();
        }


        void UpdateTimer()
        {
            if (!_started) return;
            if (pause) return;

            _second += Time.deltaTime;

            if (_second >= SECONDS_PER_MINUTE)
            {
                _second = 0;
                Minute++;

                if (Minute >= 60)
                {
                    Minute = 0;
                    Hour++;
                    EndingManager.Instance.GameWin();
                }

                UpdateTimerText();
            }
        }

        void UpdateTimerText()
        {
            GlobalTimer.text = $"{Hour:D2}:{Minute:D2}";
        }


        public void PauseTimer()
        {
            pause = true;
        }

        public void ResumeTimer()
        {
            pause = false;
        }
    }
}
