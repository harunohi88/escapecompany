using UnityEngine;
using System.Collections.Generic;

public enum DocumentType
{
    Down,
    Left,
    Right
}

public class DocumentGameManager : MonoBehaviour
{
    public static DocumentGameManager Instance;

    public float TimeLimit;
    public Player Player;
    public List<Document> DocumentPrefabList;
    public Queue<Document> DocumentQueue = new Queue<Document>();
    public string Stage = "LRDDDRRLLLDDRRDRDDLLLLDDDDDRRRDDRDLRDLD";

    private bool _status = false;
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

        // 추후에 파일에서 Stage 정보를 읽어올 수 있도록 수정
        GenerateQueue(Stage);
        GameStart();
    }

    public void GameStart()
    {
        _time = 0;
        _combo = 0;
        _totalScore = 0;
        _status = true;

        Player.GameStart();
    }

    public void GameOver()
    {
        Player.GameOver();
    }

    public void AddScore(int score)
    {
        this._totalScore += score;
    }

    public void AddCombo()
    {
        _combo++;
    }

    public void ResetCombo()
    {
        _combo = 0;
    }

    private void GenerateQueue(string stage)
    {
        Document document = null;

        foreach (char type in stage)
        {
            switch (type)
            {
                case 'D':
                    document = Instantiate(DocumentPrefabList[(int)DocumentType.Down]);
                    break;
                case 'L':
                    document = Instantiate(DocumentPrefabList[(int)DocumentType.Left]);
                    break;
                case 'R':
                    document = Instantiate(DocumentPrefabList[(int)DocumentType.Right]);
                    break;
            }
            document.gameObject.SetActive(false);
            DocumentQueue.Enqueue(document);
        }
    }

    public Document AskDocument()
    {
        Document document = null;

        Debug.Log(DocumentQueue.Count);
        if (DocumentQueue.Count > 0)
        {
            document = DocumentQueue.Dequeue();
            document.gameObject.SetActive(true);
        }
        else
        {
            GameOver();
        }
        return document;
    }

   
}
