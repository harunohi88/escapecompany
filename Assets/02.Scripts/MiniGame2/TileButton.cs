using DG.Tweening; // 버튼 클릭할 때 바운스 효과를 주기 위해 사용
using UnityEngine;
using UnityEngine.UI;

namespace MiniGameTWo
{
    public class TileButton : MonoBehaviour
    {
        private Button btn;
        private Image img;
        public bool isRed = false;

        private GameTwoManager gameManager;
        public Sprite[] sprites;

        void Awake()
        {
            btn = GetComponent<Button>();
            img = GetComponent<Image>();
            btn.onClick.AddListener(OnClick);
        }

        public void Init(bool red, GameTwoManager manager)
        {
            isRed = red;
            //img.color = red ? Color.red : Color.white;
            img.sprite = red ? sprites[0] : sprites[1];
            gameManager = manager;
        }

        public void HandleButtonClick()
        {
            OnClick();
        }

        void OnClick()
        {
            btn.interactable = false;
            if (isRed)
            {
                img.color = Color.gray;
                // 바운스 효과
                //transform.DOKill();
                transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
                gameManager.RedCleared();     // 성공 조건 체크
            }
            else
            {
                //transform.DOKill();
                transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
                gameManager.OnwrongClick();   // 실패 조건 체크
            }
        }
    }
}
