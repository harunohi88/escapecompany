using System;
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
        private bool pause = false;

        public int Hour { get; private set; } = 5;
        public int Minute { get; private set; } = 50;
        bool _isActive;

        void OnEnable()
        {
            DialogueManager.Instance.OnDialogueEnded += StartTimer;
        }
        void OnDisable()
        {
            DialogueManager.Instance.OnDialogueEnded -= StartTimer;
        }
        void Start()
        {
            _isActive = false;
            UpdateTimerText();
            UI_MiniGame1.Instance.ShowResultAction += PauseTimer;
        }

        void Update()
        {
            UpdateTimer();
        }

        void UpdateTimer()
        {
            if (!_isActive) return;
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

        void StartTimer()
        {
            _isActive = true;
        }

        public void PauseTimer()
        {
            pause = true;
            UI_MiniGame1.Instance.OnCloseButtonClicked += ResumeTimer;
        }

        public void ResumeTimer()
        {
            pause = false;
        }
    }
}
