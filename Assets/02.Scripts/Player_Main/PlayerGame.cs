using DocumentGame;
using UnityEngine;
public class PlayerGame : MonoBehaviour
{
    // 목표: 플레이어가 미니게임 2를 플레이할 수 있도록 함
    // GameTwo 태그가 달린 오브젝트 가까이 가면 카메라가 위쪽으로 이동하면서 미니게임2가 활성화된다.

    public StartButton StartButton;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GameTwo"))
        {
            StartButton.MiniGame2 = true;
        }

        if (other.gameObject.CompareTag("GameOne"))
        {
            StartButton.MiniGame1 = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("충돌 끝");
        StartButton.MiniGame1 = false;
        StartButton.MiniGame2 = false;
    }
}
