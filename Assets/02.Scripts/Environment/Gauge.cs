using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace CreateMap
{
    public class Gauge : MonoBehaviour
    {
        public Image Image;

        public float MaxTime;
        public float CurrentTime;


        // 미니게임을 성공하면 Reset
        public void Reset()
        {
            CurrentTime = 0;
        }
        void Update()
        {
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
    }
}
