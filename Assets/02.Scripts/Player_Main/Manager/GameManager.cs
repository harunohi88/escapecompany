using UnityEngine;
using UnityEngine.SceneManagement;
// 씬 전환을 위해 추가

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 싱글톤 유지
        } else if (Instance != this)
        {
            Destroy(gameObject); // 기존 인스턴스가 있다면 삭제
        }
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
