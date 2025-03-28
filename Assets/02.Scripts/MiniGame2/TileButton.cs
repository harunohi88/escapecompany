using DG.Tweening; // ��ư Ŭ���� �� �ٿ ȿ���� �ֱ� ���� ���
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

        public GameTwoManager GameManager;
        //public Sprite[] sprites;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _img = GetComponent<Image>();
            _btn.onClick.AddListener(OnClick);
        }

        public void Init(bool red, GameTwoManager manager, Sprite redSprite, Sprite whiteSprite)
        {
            isRed = red;
            //img.color = red ? Color.red : Color.white;
            Debug.Log($"{_img == null}");
            _img.sprite = red ? whiteSprite : redSprite;
            GameManager = manager;
        }

        public void HandleButtonClick()
        {
            OnClick();
        }

        void OnClick()
        {
            _btn.interactable = false;
            if (isRed)
            {
                _img.color = Color.gray;
                // �ٿ ȿ��
                //transform.DOKill();
                transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
                GameManager.RedCleared();     // ���� ���� üũ
            }
            else
            {
                //transform.DOKill();
                transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
                GameManager.OnwrongClick();   // ���� ���� üũ
            }
        }
    }
}
