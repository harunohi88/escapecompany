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
        bool _isActive = false; // 튜토리얼이 끝나야 true

        // 미니게임을 성공하면 Reset (게이지만 0으로, 진행/정지 상태는 호출한 쪽이 결정)
        public void Reset()
        {
            CurrentTime = 0;
            enabled = true;
            if (Image != null) Image.fillAmount = 0;
        }

        // 싱글톤 Awake 순서에 의존하지 않도록 구독은 Start에서 한 번만 한다.
        void Start()
        {
            DialogueManager dialogue = DialogueManager.Instance;
            if (dialogue != null && !dialogue.HasEnded)
            {
                dialogue.OnDialogueEnded += OnDialogueEnded;
            }
            else
            {
                OnDialogueEnded();
            }

            if (UI_MiniGame1.Instance != null)
            {
                // 미니게임1 결과창이 떠 있는 동안 멈추고, 닫으면 재개
                UI_MiniGame1.Instance.ShowResultAction += StopGauge;
                UI_MiniGame1.Instance.OnCloseButtonClicked += StartGauge;
            }
        }

        void OnDestroy()
        {
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.OnDialogueEnded -= OnDialogueEnded;

            if (UI_MiniGame1.Instance != null)
            {
                UI_MiniGame1.Instance.ShowResultAction -= StopGauge;
                UI_MiniGame1.Instance.OnCloseButtonClicked -= StartGauge;
            }
        }

        void OnDialogueEnded()
        {
            Activete();
            StartGauge();
        }

        void Update()
        {
            if (!_isActive) return;

            if (!IsActive) return;

            CurrentTime += Time.deltaTime;
            Image.fillAmount = CurrentTime / MaxTime;

            if (CurrentTime >= MaxTime)
            {
                IsActive = false;
                EndingManager.Instance.GameOver();
            }
        }


        public float GetPercent()
        {
            return CurrentTime / MaxTime;
        }
        public void StopGauge()
        {
            IsActive = false;
        }
        public void StartGauge()
        {
            IsActive = true;
            enabled = true;
        }

        public void Activete()
        {
            _isActive = true;
        }
    }
}
