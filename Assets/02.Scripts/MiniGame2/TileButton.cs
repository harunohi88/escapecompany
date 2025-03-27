using DG.Tweening; // ��ư Ŭ���� �� �ٿ ȿ���� �ֱ� ���� ���
using UnityEngine;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    private Button btn;
    private Image img;
    public bool isRed = false;

    private GameTwoManager gameManager;

    void Awake()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        btn.onClick.AddListener(OnClick);
    }

    public void Init(bool red, GameTwoManager manager)
    {
        isRed = red;
        img.color = red ? Color.red : Color.white;
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
            // �ٿ ȿ��
            //transform.DOKill();
            transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
            gameManager.RedCleared();     // ���� ���� üũ
        }
        else
        {
            //transform.DOKill();
            transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
            //gameManager.OnwrongClick();   // ���� ���� üũ
        }
    }
}