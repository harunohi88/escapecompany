using System;
using DocumentGame;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace CreateMap
{
    public class Gauge : MonoBehaviour
    {
        public Image Image;

        public float MaxTime;
        public float CurrentTime;
        public bool IsActive { get; set; }
        bool _isActive = false;

        // 미니게임을 성공하면 Reset
        public void Reset()
        {
            CurrentTime = 0;
            enabled = true;
            IsActive = false;
        }
        void OnEnable()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnDialogueEnded += Activete;
                DialogueManager.Instance.OnDialogueEnded += StartGauge;
                UI_MiniGame1.Instance.ShowResultAction += StopGauge;
            }
            else
            {
                Activete();
                StartGauge();
            }
            
        
        }
        void OnDisable()
        {
            if (DialogueManager.Instance == null) return;
            DialogueManager.Instance.OnDialogueEnded -= Activete;
            DialogueManager.Instance.OnDialogueEnded -= StartGauge;
            UI_MiniGame1.Instance.ShowResultAction -= StopGauge;
        }
        
        void Update()
        {
            if (!_isActive) return;

            if (!IsActive) return;

            CurrentTime += Time.deltaTime;
            Image.fillAmount = CurrentTime / MaxTime;

            if (CurrentTime >= MaxTime)
            {
                EndingManager.Instance.GameOver();
                Debug.Log("게임오버");
            }
        }


        public float GetPercent()
        {
            return CurrentTime / MaxTime;
        }
        public void StopGauge()
        {
            IsActive = false;
            UI_MiniGame1.Instance.OnCloseButtonClicked += StartGauge;
        }
        public void StartGauge()
        {
            Debug.Log("게이지 재시작!!!!!!!!");
            IsActive = true;
            enabled = true;
        }

        public void Activete()
        {
            _isActive = true;
        }
    }
}
