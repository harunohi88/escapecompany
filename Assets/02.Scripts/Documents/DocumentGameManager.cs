using System.Collections.Generic;
using CreateMap;
using UnityEngine;

namespace DocumentGame
{
    public class DocumentGameManager : MonoBehaviour
    {
        public static DocumentGameManager Instance;

        public GameObject MainPlayer; // Move on Map Player
        public GameObject Camera;
        public Gauge Gauge;
        public float TimeLimit;
        public int FeverCount;
        public float FeverTime;
        public MiniGame1Player Player; // Invisible Player
        public List<Document> DocumentPrefabList;
        public int DisplayDocumentCount;
        public List<DisplaySlot> DisplaySlot;
        public string Stage = "LRLLRRRRLLLLRRLRRRLLLRRLRRRRLLLRLLLLLRRRRLRLLLRRLRLRRRL";
        public int SuccessCount;
        readonly List<Document> _displayDocumentList = new List<Document>();

        readonly Queue<Document> _documentQueue = new Queue<Document>();
        int _combo;
        int _correctCount;
        int _faultImportant;
        int _faultTrash;
        bool _fever;
        int _feverGauge;
        float _feverTimer;
        int _maxCombo;
        bool _status;
        float _timer;
        int _totalScore;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else
            {
                Destroy(gameObject);
            }
        }

        void Update()
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
                    UI_MiniGame1.Instance.InactivateFever();
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
            Gauge.StopGauge();
            GenerateQueue(Stage);
            InitDisplay();
            GameStart();
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
            UI_MiniGame1.Instance.InactivateFever();

            Player.GameStart();
        }

        public void GameOver()
        {
            Player.GameOver();
            IsSuccess();
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
            Debug.Log("조이스틱 켜버리");
            MainPlayer.GetComponent<Player>().Stop();
            _documentQueue.Clear();
            _displayDocumentList.Clear();
            _documentQueue.Clear();
            UI_MiniGame1.Instance.InactivateCombo();
            UI_MiniGame1.Instance.InactivateFever();

            if (DialogueManager.Instance.CurrentIndex == 5)
            {
                DialogueManager.Instance.ActiveDialogueBox();
            }
        }

        public void IsSuccess()
        {
            if (_faultTrash + _faultImportant < SuccessCount)
            {
                Gauge.Reset();
                MainPlayer.GetComponent<Player>().AddStunItemNum();
            }
        }

        void ShowResult()
        {
            Debug.Log($"Total Score : {_totalScore}");
            Debug.Log($"Max Combo : {_maxCombo}");
            Debug.Log($"Play Time : {_timer.ToString("F2")}");
            Debug.Log($"����� �з��� ���� : {_correctCount}");
            Debug.Log($"���ƹ��� 1�� ��� : {_faultImportant}");
            Debug.Log($"�����ϰ� ������ ������ : {_faultTrash}");
        }

        public void Correct(int score)
        {
            if (_fever)
            {
                Fever(score);
                return;
            }
            _totalScore += score * (_combo / 10 + 1);
            ++_correctCount;
            ++_combo;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
            ++_feverGauge;
            if (_feverGauge >= FeverCount) // magic number
            {
                Debug.Log("Fever Time!!");
                UI_MiniGame1.Instance.ActivateFever();
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
            } else
            {
                ++_faultImportant;
            }
            _maxCombo = Mathf.Max(_maxCombo, _combo);
            _combo = 0;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
            _feverGauge = Mathf.Max(0, _feverGauge - 5); // magic number
        }

        void Fever(int score)
        {
            _totalScore += score * (_combo / 10 + 1); // magic number
            ++_combo;
            ++_correctCount;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
        }

        void GenerateQueue(string stage)
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
            } else
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
