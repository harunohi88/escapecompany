using TMPro;
using UnityEngine;

namespace CreateMap
{
    public class UI_Game : MonoBehaviour
    {
        public static readonly int SECONDPERMINIUTE = 20;
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
