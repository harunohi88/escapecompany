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
    public float FeverTime;
    public Player Player;
    public List<Document> DocumentPrefabList;
    public Queue<Document> DocumentQueue = new Queue<Document>();
    public string Stage = "LRDDDRRLLLDDRRDRDDLLLLDDDDDRRRDDRDLRDLD";

    private bool _status = false;
    private bool _fever = false;
    private int _totalScore = 0;
    private int _maxCombo = 0;
    private int _combo = 0;
    private int _feverGauge = 0;
    private float _timer = 0;
    private float _feverTimer = 0;

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

    private void Update()
    {
        if (!_status)
        {
            return;
        }
        _timer += Time.deltaTime;
        if (_timer >= TimeLimit)
        {
            GameOver();
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
        _timer = 0;
        _combo = 0;
        _totalScore = 0;
        _maxCombo = 0;
        _feverGauge = 0;
        _status = true;
        _fever = false;  // 함수로 빼자

        Player.GameStart();
    }

    public void GameOver()
    {
        Player.GameOver();
        _status = false;
        _maxCombo = Mathf.Max(_maxCombo, _combo);
        ShowResult();
    }

    private void ShowResult()
    {
        Debug.Log($"Total Score : {_totalScore}");
        Debug.Log($"Max Combo : {_maxCombo}");
        Debug.Log($"Play Time : {_timer.ToString("F2")}");
    }

    public void AddScore(int score)
    {
        this._totalScore += score * ((_combo / 10) + 1);
        _combo++;
    }

    public void ResetCombo()
    {
        _maxCombo = Mathf.Max(_maxCombo, _combo);
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
