using DG.Tweening;
using UnityEngine;
using CreateMap;
// DoTween 네임스페이스 추가

public class BeatEffect : MonoBehaviour
{
    public Vector3 beatScale = new Vector3(1.1f, 1.1f, 1f); // 커지는 크기
    public float beatDuration = 0.1f; // 애니메이션 지속 시간
    Vector3 originalScale;
    bool _subscribed;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        DialogueManager dialogue = DialogueManager.Instance;
        if (dialogue != null && !dialogue.HasEnded)
        {
            dialogue.OnDialogueEnded += DoBeatEffect;
            _subscribed = true;
        }
        else
        {
            DoBeatEffect();
        }
    }

    void OnDisable()
    {
        if (_subscribed && DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnded -= DoBeatEffect;
        _subscribed = false;

        transform.DOKill();
        transform.localScale = originalScale;
    }

    void DoBeatEffect()
    {
        // 크기 변화 애니메이션 (둠칫): 원래 크기 <-> beatScale 무한 반복
        transform.DOKill(); // 중복 실행 방지
        transform.localScale = originalScale;
        transform.DOScale(beatScale, beatDuration)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
