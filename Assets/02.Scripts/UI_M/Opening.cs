using System.Collections;
using UnityEngine;
public class Opening : MonoBehaviour
{
    public GameObject[] Openings;

    void Start()
    {
        StartCoroutine(OpeningSequence());

    }

    IEnumerator OpeningSequence()
    {
        // 배열이 비어있는지 확인
        if (Openings == null || Openings.Length == 0)
        {
            Debug.LogWarning("Openings 배열이 비어 있습니다!");
            yield break;
        }

        for (int i = 0; i < Openings.Length; i++)
        {
            Openings[i].SetActive(true);
            yield return new WaitForSecondsRealtime(1.2f);
        }

        GameManager.Instance.LoadScene();
    }
}
