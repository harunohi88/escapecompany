using UnityEngine;
using System.Collections.Generic;

public class DocumentGameManager : MonoBehaviour
{
    public static DocumentGameManager Instance;

    public float PlayTime;
    public List<Document> DocumentPrefabList;
    public Queue<Document> DocumentQueue = new Queue<Document>();
    public string Stage = "LRDDDRRLLLDDRRDRDDLLLLDDDDDRRRDDRDLRDLD";

    private int _totalScore = 0;
    private int _combo = 0;
    private float _time = 0;

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

    public void NewGame()
    {
        _time = 0;
        _combo = 0;
        _totalScore = 0;

        // 추후에 파일에서 Stage 정보를 읽어올 수 있도록 수정
        GenerateQueue(Stage);
    }

    public void AddScore(int score)
    {
        this._totalScore += score;
    }

    private void GenerateQueue(string stage)
    {
        foreach (char type in stage)
        {
            switch (type)
            {
                case 'D':
                    DocumentQueue.Enqueue(Instantiate(DocumentPrefabList[0]));
                    break;
                case 'L':
                    DocumentQueue.Enqueue(Instantiate(DocumentPrefabList[1]));
                    break;
                case 'R':
                    DocumentQueue.Enqueue(Instantiate(DocumentPrefabList[2]));
                    break;
            }
        }
    }
}
