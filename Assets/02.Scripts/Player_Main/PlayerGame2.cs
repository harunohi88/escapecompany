using System.Collections;
using UnityEngine;

public class PlayerGame2 : MonoBehaviour
{
    // 목표: 플레이어가 미니게임 2를 플레이할 수 있도록 함
    // GameTwo 태그가 달린 오브젝트 가까이 가면 카메라가 위쪽으로 이동하면서 미니게임2가 활성화된다.
    public GameObject miniGame2Root;
    public GameObject joyStickUI;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("충돌중이다: " + other.gameObject.name + ", 태그: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("GameTwo"))
        {
            Debug.Log("미니게임2 시작");
            StartCoroutine(EnterMiniGameTwo());
        }
    }

    IEnumerator EnterMiniGameTwo()
    {
        joyStickUI.SetActive(false);
        yield return new WaitForSeconds(2f);
        miniGame2Root.SetActive(true);
    }

}
