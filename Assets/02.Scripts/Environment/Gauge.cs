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
            // 멈춤/재개는 각 미니게임 매니저가 자기 게이지만 직접 제어한다 (플레이 중인 게임의 게이지만 멈춤)
        }

        void OnDestroy()
        {
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.OnDialogueEnded -= OnDialogueEnded;
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
