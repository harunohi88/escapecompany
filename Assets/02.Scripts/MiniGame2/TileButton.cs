using DG.Tweening; // 버튼 클릭할 때 바운스 효과를 주기 위해 사용
using UnityEngine;
using UnityEngine.UI;

namespace MiniGameTWo
{
    [System.Serializable]
    public class SpritePair
    {
        public Sprite RedSprite;
        public Sprite WhiteSprite;
    }

    public class TileButton : MonoBehaviour
    {
        private Button _btn;
        private Image _img;
        public bool isRed = false;

        [Header("버튼 사운드 설정")]
        public AudioClip SuccessSound;
        public AudioClip FailSound;
        private AudioSource _audioSource;

        public GameObject ButtonRightVFX;
        public GameObject ButtonWrongVFX;

        private GameTwoManager GameManager;
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
                //transform.DOShakeRotation(
                //    duration: 2f,
                //    strength: new Vector3(0, 0, 10f),
                //    vibrato: 5,
                //    randomness: 90,
                //    fadeOut: true
                //).SetLoops(-1);

                //_img.color = Color.gray;
                // 바운스 효과
                //transform.DOKill();
                _audioSource.PlayOneShot(SuccessSound);
                transform.DOPunchScale(Vector3.one * 0.7f, 0.3f);
                transform.DOScale(1.2f, 0.2f).SetLoops(-1, LoopType.Yoyo);
                GameObject vfx = Instantiate(ButtonRightVFX, transform.position, Quaternion.identity);
                GameManager.RegisterVFX(vfx);
                GameManager.RedCleared();     // 성공 조건 체크
            }
            else
            {
                //transform.DOKill();
                _audioSource.PlayOneShot(FailSound);
                transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
                GameObject vfx2 = Instantiate(ButtonWrongVFX, transform.position, Quaternion.identity);
                Destroy(vfx2, 0.5f);
                GameManager.OnwrongClick();   // 실패 조건 체크
            }
        }
    }
}
