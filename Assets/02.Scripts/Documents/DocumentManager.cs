using UnityEngine;
using System.Collections.Generic;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager Instance;

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
    }
}
