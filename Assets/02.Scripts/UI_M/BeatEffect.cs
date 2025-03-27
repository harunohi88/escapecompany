using DG.Tweening;
using UnityEngine;
// DoTween 네임스페이스 추가

public class BeatEffect : MonoBehaviour
{
    public Vector3 beatScale = new Vector3(1.1f, 1.1f, 1f); // 커지는 크기
    public float beatDuration = 0.1f; // 애니메이션 지속 시간
    Vector3 originalScale;

    void Start()
    {
        DoBeatEffect();
    }

    void DoBeatEffect()
    {
        // 크기 변화 애니메이션 (둠칫)
        transform.DOScale(beatScale, beatDuration)
            .SetEase(Ease.OutQuad) // 부드러운 감속 효과
            .OnComplete(() => transform.DOScale(originalScale, beatDuration).SetEase(Ease.InQuad)).SetLoops(-1, LoopType.Yoyo); // 원래 크기로 복귀
    }
}
