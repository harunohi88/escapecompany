using CreateMap;
using DocumentGame;
using UnityEngine;
public class PlayerGame : MonoBehaviour
{
    // 목표: 플레이어가 미니게임 2를 플레이할 수 있도록 함
    // GameTwo 태그가 달린 오브젝트 가까이 가면 카메라가 위쪽으로 이동하면서 미니게임2가 활성화된다.

    public StartButton StartButton;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            if (DialogueManager.Instance != null && DialogueManager.Instance.CurrentIndex == 5)
            {
                DialogueManager.Instance.NextDialogue();
                return;
            }

            // 튜토리얼 중 아직 움직이지 않는 부장에 부딪혀서 게임오버되지 않도록
            Boss boss = other.gameObject.GetComponentInParent<Boss>();
            if (boss != null && !boss.IsActive) return;

            EndingManager.Instance.GameOver();
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GameTwo"))
        {
            if (DialogueManager.Instance == null) return;
            if (DialogueManager.Instance.CurrentIndex == 1)
            {
                DialogueManager.Instance.NextDialogue();
            }
        }

        if (other.gameObject.CompareTag("GameOne"))
        {
            if (DialogueManager.Instance == null) return;
            if (DialogueManager.Instance.CurrentIndex == 3)
            {
                DialogueManager.Instance.NextDialogue();
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        StartButton.MiniGame1 = false;
        StartButton.MiniGame2 = false;
    }

    void OnTriggerStay2D(Collider2D other)
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
}
