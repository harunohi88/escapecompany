using System.Collections.Generic;
using CreateMap;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

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
        private readonly string[] _stages = new string[]
        {
            "LRLLRRRRLLLLRRLRLLLRRLLRLLLRRLLLRLRLRRRL",
            "LRLLRRRLLLRRLLRLRRLLLRRLRLRLLRLLLRLLRRLL",
            "RLLLRRRLLLRRLRLRRLLLRLLRLRLLRLRLLLRLRRRL",
            "LLRRLRRRLLLRLLRRLLLRRLRRLLLRRRRLLRLRRLLR",
            "RLLLRLLLRRRLLRLLLRRLLLRRRLLRLLLRRLRLRRRL",
            "LLRRLLRRRRLLLRLRRLLLRRLLLRRLLLRRLRLRRLLR",
            "LRLLRRRLLLRRLLRLRRLLLRRRLLLRRLLRRLRLRRLL",
            "LRRLRLRLLRLRLRRLLRLRLRRLLRLRRLRRLLRLRLRR",
            "RRLLRRLLRRLLRRLLRRLLRRLLRRLLRRLLRRLLRRLL",
            "LRLRRLRRLLLRRLRLRLLLRLRLRLRLLLLRRLLLRLRL",
            "RLLRLLRLLRLLRLLRLLRLLRLLRLLRLLRLLRLLRLLR",
            "LLRRLLRRLLRRRLLRRLLRRLLRRLLRRLLRRLLRRLLR" 
        };
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
                    Player.FeverEnd();
                    UI_MiniGame1.Instance.InactivateFever();
                    _fever = false;
                    _feverTimer = 0;
                    _feverGauge = 0;
                }
            }
        }

        bool _suspended; // 도망치느라 중단한 상태 (진행 상황 유지)

        public bool IsPlaying => _status;
        public bool IsSuspended => _suspended;

        void Start()
        {
            // 결과창을 닫으면 이 미니게임의 게이지만 다시 찬다 (다른 미니게임 게이지는 건드리지 않음)
            UI_MiniGame1.Instance.OnCloseButtonClicked += OnResultClosed;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
            if (UI_MiniGame1.Instance != null)
                UI_MiniGame1.Instance.OnCloseButtonClicked -= OnResultClosed;
        }

        void OnResultClosed()
        {
            Gauge.StartGauge();
        }

        // 새 판을 시작하거나, 중단했던 판이 있으면 이어서 한다. 시작/재개했으면 true
        public bool NewGame()
        {
            if (_status)
            {
                return false;
            }
            Vector3 newPosition = new Vector3(Camera.transform.position.x, Camera.transform.position.y, transform.position.z);
            transform.parent.transform.position = newPosition;
            Gauge.StopGauge();

            if (_suspended)
            {
                Resume();
                return true;
            }

            GenerateQueue(_stages[Random.Range(0, _stages.Length)]);
            InitDisplay();
            GameStart();
            return true;
        }

        // 도망: 남은 문서·점수·콤보·시간·피버를 그대로 두고 화면만 치운다. 이 미니게임의 게이지는 다시 찬다
        public void Suspend()
        {
            if (!_status)
            {
                return;
            }
            _status = false;
            _suspended = true;

            Player.Suspend();
            foreach (Document document in _displayDocumentList)
            {
                if (document != null) document.gameObject.SetActive(false);
            }
            UI_MiniGame1.Instance.InactivateCombo();
            UI_MiniGame1.Instance.InactivateFever();

            Gauge.StartGauge();
            MainPlayer.GetComponent<Player>().Stop();
        }

        void Resume()
        {
            _suspended = false;
            _status = true;

            RefreshDisplay();
            UI_MiniGame1.Instance.ActivateCombo();
            UI_MiniGame1.Instance.RefreshComboText(_combo);
            if (_fever) UI_MiniGame1.Instance.ActivateFever();

            Player.Resume(_fever);
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
            _suspended = false;
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
            MainPlayer.GetComponent<Player>().Stop();
            ClearDocuments();
            UI_MiniGame1.Instance.InactivateCombo();
            UI_MiniGame1.Instance.InactivateFever();

            // 튜토리얼: 미니게임1을 마치면 미리 넣어 둔 '부장 기절' 안내를 보여 준다
            if (DialogueManager.IsStep(TutorialStep.StunBoss))
            {
                DialogueManager.Instance.ActiveDialogueBox();
            }
        }

        // 화면에 남은 문서와 대기열의 (비활성) 문서 오브젝트를 파괴. 리스트만 비우면 화면에 남고 매 판 40개씩 쌓였음
        void ClearDocuments()
        {
            foreach (Document document in _displayDocumentList)
            {
                if (document != null) Destroy(document.gameObject);
            }
            foreach (Document document in _documentQueue)
            {
                if (document != null) Destroy(document.gameObject);
            }
            _displayDocumentList.Clear();
            _documentQueue.Clear();
        }

        public void IsSuccess()
        {
            if (_faultTrash + _faultImportant < SuccessCount)
            {
                Gauge.Reset();
                MainPlayer.GetComponent<Player>().AddStunItemNum();
            }
        }

        public void Correct(int score)
        {
            if (_fever)
            {
                Fever(score);
                return;
            }

            ProCamera2DShake.Instance.Shake(ProCamera2DShake.Instance.ShakePresets[2]);

            _totalScore += score * (_combo / 10 + 1);
            ++_correctCount;
            ++_combo;
            UI_MiniGame1.Instance.RefreshComboText(_combo);
            ++_feverGauge;
            if (_feverGauge >= FeverCount) // magic number
            {
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

            ProCamera2DShake.Instance.Shake(ProCamera2DShake.Instance.ShakePresets[1]);

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
            ProCamera2DShake.Instance.Shake(ProCamera2DShake.Instance.ShakePresets[3]);
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
