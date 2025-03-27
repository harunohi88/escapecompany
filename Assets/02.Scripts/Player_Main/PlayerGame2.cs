using System.Collections;
using UnityEngine;

public class PlayerGame2 : MonoBehaviour
{
    // ��ǥ: �÷��̾ �̴ϰ��� 2�� �÷����� �� �ֵ��� ��
    // GameTwo �±װ� �޸� ������Ʈ ������ ���� ī�޶� �������� �̵��ϸ鼭 �̴ϰ���2�� Ȱ��ȭ�ȴ�.
    public GameObject miniGame2Root;
    public GameObject joyStickUI;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("�浹���̴�: " + other.gameObject.name + ", �±�: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("GameTwo"))
        {
            Debug.Log("�̴ϰ���2 ����");
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
