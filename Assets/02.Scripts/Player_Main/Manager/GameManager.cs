using UnityEngine;
using UnityEngine.SceneManagement;
// 씬 전환을 위해 추가

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    void Awake()
    {
        // 씬마다 GameManager가 있고, 엔딩의 다시하기 버튼이 그 씬의 GameManager를 직접 참조한다.
        // DontDestroyOnLoad로 이전 씬 것을 남기면 새 씬의 GameManager가 파괴되어 버튼이 끊기므로 씬 단위로 둔다.
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0; // VSync 비활성화
        Application.targetFrameRate = 120; // 프레임을 120으로 고정
    }


    // 씬 전환 함수
    public void LoadScene()
    {
        SceneManager.LoadScene("MainCreateMap");
    }

    public void LoadReplayScene()
    {
        SceneManager.LoadScene("MainReplayMap");
    }
}
