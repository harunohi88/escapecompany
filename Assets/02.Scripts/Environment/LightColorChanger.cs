using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CreateMap
{
    public class LightColorChanger : MonoBehaviour
    {
        public Gradient dayNightGradient; // 시간에 따른 색상 변화
        public UI_Game gameTime; // UI_Game 참조
        public Light2D sceneLight;

        bool isActive = false;

        // 싱글톤 Awake 순서에 의존하지 않도록 Start에서 구독
        void Start()
        {
            DialogueManager dialogue = DialogueManager.Instance;
            if (dialogue != null && !dialogue.HasEnded)
            {
                dialogue.OnDialogueEnded += Activate;
            }
            else
            {
                isActive = true;
            }
        }

        void OnDestroy()
        {
            if (DialogueManager.Instance == null) return;
            DialogueManager.Instance.OnDialogueEnded -= Activate;
        }

        void Update()
        {
            if (!isActive) return;

            if (gameTime == null || sceneLight == null)
                return;

            float timeFactor = (gameTime.Hour - 5) / 1.0f + gameTime.Minute / 60.0f;
            timeFactor = Mathf.Clamp01(timeFactor); // 0~1 범위로 제한

            sceneLight.color = dayNightGradient.Evaluate(timeFactor);
        }

        void Activate()
        {
            isActive = true;
        }
    }
}
