using UnityEngine;

namespace CreateMap
{
    public class DoomChitEffect : MonoBehaviour
    {
        public Vector3 targetScale = new Vector3(1f, 1.2f, 1f); // 커질 크기
        public float rotationAmount = 10f; // Z축 회전 각도 (±10)
        public float lerpSpeed = 2.0f; // 변화 속도
        Quaternion originalRotation;

        Vector3 originalScale;
        float timer;

        void Start()
        {
            originalScale = transform.localScale;
            originalRotation = transform.rotation;
        }

        protected void Update()
        {
            timer += Time.deltaTime * lerpSpeed;
            float t = Mathf.PingPong(timer, 1); // 0~1 반복

            // 크기 둠칫
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            // Z축 회전 둠칫 (-10° ↔ 10°)
            float zRotation = Mathf.Sin(timer * Mathf.PI) * rotationAmount;
            transform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
    }
}
