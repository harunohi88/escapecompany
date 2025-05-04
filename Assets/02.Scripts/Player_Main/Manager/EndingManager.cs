using UnityEngine;
public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;
    public GameObject EndingUI; // Ending UI를 담을 배
    public GameObject[] Ending;

    bool isEnd;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 싱글톤 유지
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 기존 인스턴스가 있다면 삭제
        }
    }
    public void GameOver()
    {
        // Time.timeScale = 0;
        if (isEnd) return;
        isEnd = true;
        EndingUI.SetActive(true);
        Ending[1].SetActive(true);
        BGMManager.Instance.GameOver();
    }

    public void GameWin()
    {
        if (isEnd) return;
        isEnd = true;

        // Time.timeScale = 0;
        EndingUI.SetActive(true);
        Ending[0].SetActive(true);
        BGMManager.Instance.GameWin();
    }
}
