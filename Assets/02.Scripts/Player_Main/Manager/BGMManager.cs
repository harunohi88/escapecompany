using UnityEngine;
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    public AudioSource[] BGMS;

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
}
