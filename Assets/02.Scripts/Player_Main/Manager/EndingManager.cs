using UnityEngine;
public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;
    public GameObject BackGround;
    public GameObject[] Ending;


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
    public void GameOver()
    {
        // Time.timeScale = 0;
        BackGround.SetActive(true);
        Ending[1].SetActive(true);
    }

    public void GameWin()
    {
        // Time.timeScale = 0;
        BackGround.SetActive(true);
        Ending[0].SetActive(true);
    }
}
