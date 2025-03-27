using TMPro;
using UnityEngine;

namespace CreateMap
{
    public class UI_Game : MonoBehaviour
    {
        static readonly int SECONDPERMINIUTE = 60;
        public TextMeshProUGUI GlobalTimmer;

        int _minute = 50;
        float _second;
        void Start()
        {
            GlobalTimmer.text = $"05 : {_minute}";
        }
        void Update()
        {
            UpdateTimer();
        }

        void UpdateTimer()
        {
            if (_minute == 60)
            {
                GlobalTimmer.text = "06 : 00";
            }
            _second += Time.deltaTime;
            if (_second >= SECONDPERMINIUTE)
            {
                _second = 0;
                _minute++;
                GlobalTimmer.text = $"05 : {_minute}";
            }
        }
    }
}
