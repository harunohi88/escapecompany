using TMPro;
using UnityEngine;

namespace CreateMap
{
    public class UI_Game : MonoBehaviour
    {
        public static readonly int SECONDS_PER_MINUTE = 8;
        public TextMeshProUGUI GlobalTimer;

        float _second;

        public int Hour { get; private set; } = 5;
        public int Minute { get; private set; } = 50;

        void Start()
        {
            UpdateTimerText();
        }

        void Update()
        {
            UpdateTimer();
        }

        void UpdateTimer()
        {
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
            GlobalTimer.text = $"{Hour:D2} : {Minute:D2}";
        }
    }
}
