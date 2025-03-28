using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using CreateMap;

namespace DocumentGame
{
    public class DocumentGameManager : MonoBehaviour
    {
        public static DocumentGameManager Instance;

        public GameObject MainPlayer; // Move on Map Player
        public GameObject Camera;
        public GameObject Joystick;
        public float TimeLimit;
        public int FeverCount;
        public float FeverTime;
        public MiniGame1Player Player; // Invisible Player
        public List<Document> DocumentPrefabList;
        public int DisplayDocumentCount;
        public List<DisplaySlot> DisplaySlot;
        public string Stage = "LRLLRRRRLLLLRRLRRRLLLRRLRRRRLLLRLLLLLRRRRLRLLLRRLRLRRRL";

        private Queue<Document> _documentQueue = new Queue<Document>();
        private List<Document> _displayDocumentList = new List<Document>();
        private bool _status = false;
        private bool _fever = false;
        private int _totalScore = 0;
        private int _maxCombo = 0;
        private int _combo = 0;
        private int _feverGauge = 0;
        private float _timer = 0;
        private float _feverTimer = 0;
        private int _faultTrash = 0;
        private int _faultImportant = 0;
        private int _correctCount = 0;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
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

            if (_fever)
            {
                _feverTimer += Time.deltaTime;
                if (_feverTimer >= FeverTime)
                {
                    Debug.Log("Fever Time End!!");
                    Player.FeverEnd();
                    _fever = false;
                    _feverTimer = 0;
                    _feverGauge = 0;
                }
            }
        }

        public void NewGame()
        {
            if (_status)
            {
                return;
            }
            Vector3 newPosition = new Vector3(Camera.transform.position.x, Camera.transform.position.y, transform.position.z);
            transform.parent.transform.position = newPosition;
            GenerateQueue(Stage);
            InitDisplay();
            GameStart();
            Joystick.SetActive(false);
        }

        public void InitDisplay()
        {
            for (int i = 0; i < DisplayDocumentCount; ++i)
            {
                AddDisplay();
            }
            RefreshDisplay();
        }

        public void GameStart()
        {
            _timer = 0;
            _combo = 0;
            _totalScore = 0;
            _maxCombo = 0;
            _feverGauge = 0;
            _correctCount = 0;
            _faultTrash = 0;
            _faultImportant = 0;
            _status = true;
            _fever = false;
            _feverTimer = 0;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
            UI_MiniGame1.Instance.ActivateCombo();

            Player.GameStart();
        }

        public void GameOver()
        {
            Player.GameOver();
            _maxCombo = Mathf.Max(_maxCombo, _combo);
            UI_MiniGame1.Instance.ShowResult(_totalScore, _maxCombo, _timer, _correctCount, _faultImportant, _faultTrash);
            _timer = 0;
            _combo = 0;
            _totalScore = 0;
            _maxCombo = 0;
            _feverGauge = 0;
            _correctCount = 0;
            _faultTrash = 0;
            _faultImportant = 0;
            _status = false;
            _fever = false;
            _feverTimer = 0;
            Joystick.SetActive(true);
            MainPlayer.GetComponent<CreateMap.Player>().Stop();
            _documentQueue.Clear();
            _displayDocumentList.Clear();
            _documentQueue.Clear();
            UI_MiniGame1.Instance.InactivateCombo();
        }

        private void ShowResult()
        {
            Debug.Log($"Total Score : {_totalScore}");
            Debug.Log($"Max Combo : {_maxCombo}");
            Debug.Log($"Play Time : {_timer.ToString("F2")}");
            Debug.Log($"제대로 분류한 서류 : {_correctCount}");
            Debug.Log($"갈아버린 1급 기밀 : {_faultImportant}");
            Debug.Log($"소중하게 보관한 쓰레기 : {_faultTrash}");
        }

        public void Correct(int score)
        {
            if (_fever)
            {
                Fever(score);
                return;
            }
            this._totalScore += score * ((_combo / 10) + 1);
            ++_correctCount;
            ++_combo;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
            ++_feverGauge;
            if (_feverGauge >= FeverCount) // magic number
            {
                Debug.Log("Fever Time!!");
                _fever = true;
                Player.Fever();
            }
        }

        public void Wrong(int score, DocumentType type)
        {
            if (_fever)
            {
                Fever(score);
                return;
            }

            if (type == DocumentType.Trash)
            {
                ++_faultTrash;
            }
            else
            {
                ++_faultImportant;
            }
            _maxCombo = Mathf.Max(_maxCombo, _combo);
            _combo = 0;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
            _feverGauge = Mathf.Max(0, _feverGauge - 5); // magic number
        }

        private void Fever(int score)
        {
            this._totalScore += score * ((_combo / 10) + 1); // magic number
            ++_combo;
            ++_correctCount;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
        }

        private void GenerateQueue(string stage)
        {
            Document document = null;

            foreach (char type in stage)
            {
                switch (type)
                {
                    case 'L':
                        document = Instantiate(DocumentPrefabList[(int)DocumentType.Important]);
                        break;
                    case 'R':
                        document = Instantiate(DocumentPrefabList[(int)DocumentType.Trash]);
                        break;
                }
                document.gameObject.SetActive(false);
                _documentQueue.Enqueue(document);
            }
        }

        public void AddDisplay()
        {
            if (_documentQueue.Count > 0)
            {
                _displayDocumentList.Add(_documentQueue.Dequeue());
            }
            else
            {
                _displayDocumentList.Add(null);
            }
        }

        public void RefreshDisplay()
        {
            for (int i = 0; i < DisplayDocumentCount; ++i)
            {
                DisplaySlot[i].DisplayDocument(_displayDocumentList[i]);
            }
        }

        public Document GetDocument()
        {
            Document document = _displayDocumentList[0];
            if (document == null)
            {
                GameOver();
                return null;
            }
            return document;
        }

        public void Process()
        {
            _displayDocumentList.RemoveAt(0);
            AddDisplay();
            RefreshDisplay();
        }
    }
}
