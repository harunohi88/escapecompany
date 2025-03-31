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

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            _originalPos = TimerText.transform.localPosition;
            timer = GameTime;
        }

        void Update()
        {
            if (!isGameStart) return;
            if (isGameOver) return;

            timer -= Time.deltaTime;
            TimerText.text = $"{Mathf.Ceil(timer)}";
            UpdateTimerColor();

            if (!fuseStarted && timer < 5f)
            {
                TriggerFuseEffects();
            }

            if (timer <= 0)
            {
                Debug.Log("Timmer 0");
                GameOver(false);
            }
        }

        public void InitGame()
        {
            if (isGameStart) return;
            isGameStart = true;

            DOTween.KillAll(); // 모든 tween 초기화 (중복 방지)
            enabled = true;
            isGameOver = false;
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

        public void RedCleared()
        {
            if (isGameOver) return;

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
                    Debug.Log("성공임");
                    GameOver(true);
                } else
                {
                    StartCoroutine(ResetBoard());
                }
            }
        }

        public void OnwrongClick()
        {
            if (isGameOver) return;

            if (currentHealth <= 0)
            {
                Debug.Log("체력 0임");
                GameOver(false);
                return;
            }

            HeartPrefab[currentHealth - 1].GetComponent<Animator>().Play("heart");
            currentHealth--;

            if (currentHealth <= 0)
            {
                Debug.Log("체력 02임");
                GameOver(false);
            } else
            {
                _audioSource.Play();
                StartCoroutine(ResetBoard());
            }
        }

        IEnumerator ResetBoard()
        {
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
            ResetTimerEffects();
            ProCamera2DShake.Instance.Shake(ProCamera2DShake.Instance.ShakePresets[6]);
#if UNITY_ANDROID && !UNITY_EDITOR
    Handheld.Vibrate();
#endif

            Debug.Log("Game Over");
            player.Stop();
            if (success)
            {
                Debug.Log("게임 클리어 처리");
                player.AddStunItemNum();
                Gauge.Reset();
            } else
            {
                Debug.Log("게임 실패 처리 - 게이지 시작!");
                Gauge.StartGauge();
            }

            ClearTilesAndVFX();
            isGameOver = false;
            UIPrefab.SetActive(false);
            enabled = false;
            isGameStart = false;

            if (DialogueManager.Instance.CurrentIndex == 2)
            {
                DialogueManager.Instance.ActiveDialogueBox();
                DialogueManager.Instance.NextDialogue();
            }
        }

        void ClearTilesAndVFX()
        {
            foreach (Transform tile in UIMiniTwo.GridParent)
            {
                Destroy(tile.gameObject);
            }

            foreach (GameObject vfx in VFXs)
            {
                Destroy(vfx);
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
