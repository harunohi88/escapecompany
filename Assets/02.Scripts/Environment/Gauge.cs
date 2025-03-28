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

        // 미니게임을 성공하면 Reset
        public void Reset()
        {
            CurrentTime = 0;
        }
        void Update()
        {
            if (IsActive) return;
            CurrentTime += Time.deltaTime;
            Image.fillAmount = CurrentTime / MaxTime;

            if (CurrentTime >= MaxTime)
            {
                // GameManager.Instance.GameOver();
                Debug.Log("게임오버");
            }
        }


        public float GetPercent()
        {
            return CurrentTime / MaxTime;
        }
        public void StopGauge()
        {
            IsActive = true;
            UI_MiniGame1.Instance.OnCloseButtonClicked += StartGauge;
        }
        public void StartGauge()
        {
            IsActive = false;
        }
    }
}
