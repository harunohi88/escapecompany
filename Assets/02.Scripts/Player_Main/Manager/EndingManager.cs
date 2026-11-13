using UnityEngine;
public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;
    public GameObject EndingUI; // Ending UI를 담을 배
    public GameObject[] Ending;

    bool isEnd;

    public bool IsEnd => isEnd;

    void Awake()
    {
        // 엔딩 UI는 씬마다 따로 있으므로 씬을 넘어 유지하지 않는다.
        // (DontDestroyOnLoad로 유지하면 다시하기 후 isEnd가 true로 남고 EndingUI 참조가 끊겨 엔딩이 안 뜸)
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
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
