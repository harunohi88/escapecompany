using System;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// 버튼 클릭할 때 바운스 효과를 주기 위해 사용

namespace MiniGameTWo
{
    [Serializable]
    public class SpritePair
    {
        public Sprite RedSprite;
        public Sprite WhiteSprite;
    }

    public class TileButton : MonoBehaviour
    {
        public bool isRed;

        [Header("버튼 사운드 설정")]
        public AudioClip SuccessSound;
        public AudioClip FailSound;

        public GameObject ButtonRightVFX;
        public GameObject ButtonWrongVFX;
        AudioSource _audioSource;
        Button _btn;
        Image _img;

        GameTwoManager GameManager;

        //public Sprite[] sprites;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _img = GetComponent<Image>();
            _audioSource = gameObject.GetComponent<AudioSource>();

            // _audioSource = GameObject.GetComponent<AudioSource>();
            _btn.onClick.AddListener(OnClick);
        }

        public void Init(bool red, GameTwoManager manager, Sprite redSprite, Sprite whiteSprite)
        {
            isRed = red;

            //img.color = red ? Color.red : Color.white;
            _img.sprite = red ? whiteSprite : redSprite;
            GameManager = manager;

            // 랜덤 각도 범위 (ex: -10 ~ 10도 사이)
            float randomAngle = Random.Range(5f, 15f); // 흔들림 크기
            float randomDuration = Random.Range(0.8f, 2f); // 흔들림 속도
            float randomDelay = Random.Range(0f, 1.5f); // 타이밍 차이

            // 랜덤 방향 (왼쪽 또는 오른쪽부터 시작)
            float targetAngle = Random.value > 0.5f ? randomAngle : -randomAngle;

            transform.DORotate(new Vector3(0, 0, targetAngle), randomDuration).SetDelay(randomDelay).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

            //transform.DORotate(new Vector3(0, 0, 10f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        public void HandleButtonClick()
        {
            OnClick();
        }

        void OnClick()
        {
            _btn.interactable = false;
            Color randomColor = Color.HSVToRGB(Random.Range(0f, 1f), 0.6f, 1f);
            _img.DOColor(randomColor, 0.3f);

            if (isRed)
            {

                _audioSource.PlayOneShot(SuccessSound, 0.4f);
                transform.DOPunchScale(Vector3.one * 0.7f, 0.3f);
                transform.DOScale(1.2f, 0.2f).SetLoops(-1, LoopType.Yoyo);
                GameObject vfx = Instantiate(ButtonRightVFX, transform.position, Quaternion.identity);
                GameManager.RegisterVFX(vfx);
                GameManager.RedCleared(); // 성공 조건 체크
            } else
            {
                //transform.DOKill();
                ProCamera2DShake.Instance.Shake(ProCamera2DShake.Instance.ShakePresets[6]);
                Handheld.Vibrate(); // 진동
                _audioSource.PlayOneShot(FailSound, 0.4f);
                transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
                GameObject vfx2 = Instantiate(ButtonWrongVFX, transform.position, Quaternion.identity);
                Destroy(vfx2, 0.5f);
                GameManager.OnwrongClick(); // 실패 조건 체크
            }
        }
    }
}
