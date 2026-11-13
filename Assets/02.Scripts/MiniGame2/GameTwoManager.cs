using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using CreateMap;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MiniGameTWo
{
    public class GameTwoManager : MonoBehaviour
    {
        const int maxSuccess = 3;

        public Player player;
        public UIMiniTwo UIMiniTwo;
        public TextMeshProUGUI TimerText;
        public GameObject TimerIcon;
        public float GameTime = 10f;
        public Gauge Gauge;

        [Header("체력시스템")]
        public GameObject[] HeartPrefab;
        public GameObject[] GoalPrefab;
        public GameObject BombPrefab;
        public GameObject UIPrefab;
        public TileButton TileButton;

        public AudioSource _audioSource;
        public List<GameObject> VFXs;
        public bool isGameOver;
        public bool isGameStart;

        Vector3 _originalPos;

        int currentHealth;
        bool fuseStarted;
        int redCount;
        int successCount;
        float timer;
        bool isResetting; // 보드 교체 연출 중에는 클릭/타이머 무시
        bool isSuspended; // 도망치느라 중단한 상태 (하트·라운드·타일·남은 시간 유지)

        public bool IsPlaying => isGameStart && !isSuspended;
        public bool IsSuspended => isSuspended;

        void Awake()
        {
            // 루트가 꺼져 있다가 SetActive(true) 직후 InitGame이 호출되면 Start보다 먼저 실행되므로 Awake에서 저장
            _originalPos = TimerText.transform.localPosition;
            timer = GameTime;
        }

        void Start()
        {
            if (player == null) player = GameObject.Find("Player").GetComponent<Player>();
        }

        void Update()
        {
            if (!isGameStart) return;
            if (isGameOver) return;
            if (isResetting) return;
            if (isSuspended) return;

            timer -= Time.deltaTime;
            TimerText.text = $"{Mathf.Ceil(timer)}";
            UpdateTimerColor();

            if (!fuseStarted && timer < 5f)
            {
                TriggerFuseEffects();
            }

            if (timer <= 0)
            {
                GameOver(false);
            }
        }

        // 도망: 보드와 진행 상황은 그대로 두고 화면만 숨긴다. 이 미니게임의 게이지는 다시 찬다
        public void Suspend()
        {
            if (!isGameStart || isGameOver || isSuspended) return;
            isSuspended = true;

            UIPrefab.SetActive(false);
            Gauge.StartGauge();
            player.Stop();
        }

        void Resume()
        {
            isSuspended = false;

            UIPrefab.SetActive(true);
            Gauge.StopGauge();
            // UI를 껐다 켜면 하트 애니메이터가 초기 상태로 돌아가므로 남은 하트/성공 표시를 다시 맞춘다
            ResetHearts();
            ResetGoals();
        }

        // 새 판을 시작하거나, 중단했던 판이 있으면 이어서 한다
        public void InitGame()
        {
            if (isSuspended)
            {
                Resume();
                return;
            }
            if (isGameStart) return;
            isGameStart = true;

            // DOTween.KillAll()은 다른 UI(BeatEffect 등)의 무한 트윈까지 죽이므로 미니게임2 것만 정리
            StopAllCoroutines();
            ClearTilesAndVFX();
            enabled = true;
            isGameOver = false;
            isResetting = false;
            currentHealth = 3;
            successCount = 0;
            timer = GameTime;
            redCount = 0;
            fuseStarted = false;

            ResetHearts();
            ResetGoals();
            ResetTimerEffects();

            Gauge.StopGauge();
            BombPrefab.SetActive(false);
            UIPrefab.SetActive(true);

            TimerIcon.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            UIMiniTwo.InitGame();
        }

        void UpdateTimerColor()
        {
            float timeRatio = timer / GameTime;

            if (timeRatio > 0.6f) TimerText.color = Color.white;
            else if (timeRatio > 0.4f) TimerText.color = Color.yellow;
            else if (timeRatio > 0.2f) TimerText.color = new Color(1f, 0.5f, 0f);
            else TimerText.color = Color.red;
        }

        void TriggerFuseEffects()
        {
            BombPrefab.SetActive(true);
            fuseStarted = true;

            TimerText.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            TimerText.transform.DOShakePosition(0.3f, 10f).SetLoops(-1, LoopType.Restart);
            TimerIcon.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }

        public void RegisterTile(bool isRed)
        {
            if (isRed) redCount++;
        }

        public void RegisterVFX(GameObject vfx)
        {
            VFXs.Add(vfx);
        }

        void UnRegisterVFX()
        {
            VFXs.Clear();
        }

        public bool CanClick => isGameStart && !isGameOver && !isResetting && !isSuspended;

        public void RedCleared()
        {
            if (!CanClick) return;

            redCount--;
            if (redCount <= 0)
            {
                successCount++;
                if (successCount <= GoalPrefab.Length)
                {
                    GoalPrefab[successCount - 1].SetActive(true);
                }

                if (successCount >= maxSuccess)
                {
                    GameOver(true);
                } else
                {
                    StartCoroutine(ResetBoard());
                }
            }
        }

        public void OnwrongClick()
        {
            if (!CanClick) return;

            if (currentHealth <= 0)
            {
                GameOver(false);
                return;
            }

            HeartPrefab[currentHealth - 1].GetComponent<Animator>().Play("heart");
            currentHealth--;

            if (currentHealth <= 0)
            {
                GameOver(false);
            } else
            {
                _audioSource.Play();
                StartCoroutine(ResetBoard());
            }
        }

        IEnumerator ResetBoard()
        {
            isResetting = true;
            ResetTimerEffects();

            foreach (Transform tile in UIMiniTwo.GridParent)
            {
                tile.DORotate(new Vector3(0, 0, Random.Range(-180f, 180f)), 0.4f);
                tile.DOScale(0f, 0.4f);
            }

            yield return new WaitForSeconds(0.6f);
            ClearTilesAndVFX();

            yield return new WaitForSeconds(0.2f);
            StartNewRound();
            isResetting = false;
        }

        void StartNewRound()
        {
            redCount = 0;
            timer = GameTime;
            fuseStarted = false;

            ResetHearts();
            ResetGoals();
            ResetTimerEffects();
            UIMiniTwo.InitGame();

            TimerIcon.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }

        void GameOver(bool success)
        {
            isGameOver = true;
            StopAllCoroutines(); // 진행 중인 ResetBoard가 게임 종료 후 새 타일을 만들지 않도록
            isResetting = false;
            isSuspended = false;
            ResetTimerEffects();
            ProCamera2DShake.Instance.Shake(ProCamera2DShake.Instance.ShakePresets[6]);
#if UNITY_ANDROID && !UNITY_EDITOR
    Handheld.Vibrate();
#endif

            player.Stop();
            if (success)
            {
                player.AddStunItemNum();
                Gauge.Reset();
                Gauge.StartGauge(); // 0부터 다시 차오르게 (이전엔 멈춘 채로 남음)
            } else
            {
                Gauge.StartGauge();
            }

            ClearTilesAndVFX();
            isGameOver = false;
            UIPrefab.SetActive(false);
            enabled = false;
            isGameStart = false;

            // 튜토리얼: 미니게임2를 마치면 "잘했어!! 다음은 박스 앞으로" 대사로 진행
            if (DialogueManager.IsStep(TutorialStep.PlayMiniGame2))
            {
                DialogueManager.Instance.ActiveDialogueBox();
                DialogueManager.Instance.NextDialogue();
            }
        }

        void ClearTilesAndVFX()
        {
            foreach (Transform tile in UIMiniTwo.GridParent)
            {
                tile.DOKill(); // 무한 루프 트윈이 파괴된 오브젝트를 붙잡지 않도록
                Destroy(tile.gameObject);
            }

            foreach (GameObject vfx in VFXs)
            {
                if (vfx != null) Destroy(vfx);
            }
            UnRegisterVFX();
        }

        void ResetHearts()
        {
            foreach (GameObject heart in HeartPrefab)
                heart.SetActive(false);

            for (int i = 0; i < currentHealth; i++)
            {
                HeartPrefab[i].SetActive(true);
            }
        }

        void ResetGoals()
        {
            foreach (GameObject goal in GoalPrefab)
                goal.SetActive(false);

            for (int i = 0; i < successCount; i++)
            {
                GoalPrefab[i].SetActive(true);
            }
        }

        void ResetTimerEffects()
        {
            TimerText.transform.DOKill();
            TimerText.transform.localScale = Vector3.one;
            TimerText.transform.localPosition = _originalPos;

            TimerIcon.transform.DOKill();
            TimerIcon.transform.localScale = Vector3.one;
        }
    }
}
