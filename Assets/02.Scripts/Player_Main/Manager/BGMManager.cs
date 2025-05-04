using System;
using UnityEngine;
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    public AudioSource BGMSource;
    public AudioClip[] BGMClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject); // 기존 인스턴스가 있다면 삭제
        }
    }

    private void Start()
    {
        BGMSource.clip = BGMClips[0];
        BGMSource.Play();
    }
    public void GameOver()
    {
        BGMSource.clip = BGMClips[1];
        BGMSource.Play();
    }

    public void GameWin()
    {
        BGMSource.clip = BGMClips[2];
        BGMSource.Play();
    }
}
