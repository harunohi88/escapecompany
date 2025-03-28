using CreateMap;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MiniGameTWo
{


    public class GameTwoManager : MonoBehaviour
    {
        public Player player;

        const int maxSuccess = 3;
        public UIMiniTwo UIMiniTwo;
        public TextMeshProUGUI TimerText;
        public GameObject TimerIcon;
        public float GameTime = 10f;


        [Header("체력시스템")]
        public GameObject[] HeartPrefab;
        public GameObject[] GoalPrefab;
        public GameObject BombPrefab;
        public GameObject UIPrefab;
        private Vector3 _originalPos; // 타이머 텍스트 위치 저장용
        public TileButton TileButton;

        public AudioSource _audioSource;


        int currentHealth = 3;
        bool fuseStarted;
        bool isGameOver;
        int redCount;
        int successCount;

        float timer;

        public List<GameObject> VFXs;

        void Start()
        {
            _originalPos = TimerText.transform.localPosition;
            StartNewRound();

            //timer = GameTime;
            //UIMiniTwo.InitGame();  // 게임 시작
            BombPrefab.SetActive(false);
            player = GameObject.Find("Player").GetComponent<Player>();
            //Debug.Log("bomb 끔.");

            // 게임 시작하자마자 시계 아이콘 반짝임 연출 추가
            TimerIcon.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        void Update()
        {
            timer -= Time.deltaTime;
            TimerText.text = $"{Mathf.Ceil(timer)}";


            // 제한시간에 가까이갈수록 타이머텍스트 색상 변경
            float timeRatio = timer / GameTime;

            if (timeRatio > 0.6f)
            {
                TimerText.color = Color.white;
            }
            else if (timeRatio > 0.4f)
            {
                TimerText.color = Color.yellow;
            }
            else if (timeRatio > 0.2f)
            {
                TimerText.color = new Color(1f, 0.5f, 0f);
            }
            else
            {
                TimerText.color = Color.red;
            }


            if (!fuseStarted && timer < 5f)
            {
                //Debug.Log("폭탄 발동!");
                BombPrefab.SetActive(true);
                fuseStarted = true;
                //BombPrefab.GetComponentInChildren<Animator>().Play("BombSystem");  //// bomprefab 합치고 작업해!

                // 시계 아이콘 반짝임 연출 추가
                TimerIcon.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);

                // 시간 텍스트 연출 추가
                TimerText.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                TimerText.transform.DOShakePosition(0.3f, 10f, 10, 90f, false, true).SetLoops(-1, LoopType.Restart);


            }
            if (timer <= 0)
            {
                Debug.Log("실패!");
                isGameOver = true;

                ResetTimerEffects(); // 게임 종료 직전에 이펙트 처리
                UIPrefab.SetActive(false);
                player.Stop();
                enabled = false;
            }
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
            if (isGameOver)
            {
                return;
            }

            redCount--;
            if (redCount <= 0)
            {
                successCount++;
                Debug.Log($"성공! 남은 목표: {maxSuccess - successCount}");
                if (successCount <= GoalPrefab.Length)
                {
                    GoalPrefab[successCount - 1].SetActive(true);
                }

                if (successCount >= maxSuccess)
                {
                    Debug.Log("게임 클리어!");
                    isGameOver = true;
                    UIPrefab.SetActive(false);
                    player.Stop();
                    enabled = false;
                }
                else
                {
                    StartCoroutine(ResetBoard());
                }
            }
        }

        public void OnwrongClick()
        {
            if (isGameOver)
            {
                return;
            }

            if (currentHealth <= 0)
            {
                // 이미 체력이 0인 상태에서 잘못 누름 → 즉시 게임 종료 처리
                Debug.Log("게임 종료: 기회 소진");
                isGameOver = true;
                UIPrefab.SetActive(false);
                player.Stop();
                enabled = false;
                foreach (GameObject vfx in VFXs)
                {
                    Destroy(vfx);
                }
                UnRegisterVFX();
                return;
            }


            Debug.Log($"{currentHealth}");

            // 하트 애니메이션 (SetTrigger)
            HeartPrefab[currentHealth - 1].GetComponent<Animator>().Play("heart");

            // 하트 감소
            currentHealth--;
            if (currentHealth <= 0)
            {
                // 하트 전부 소진 → 게임 종료 처리
                Debug.Log("하트 0: 게임 종료");
                enabled = false;
                isGameOver = true;
                UIPrefab.SetActive(false);
                player.Stop();
                foreach (GameObject vfx in VFXs)
                {
                    Destroy(vfx);
                }
                UnRegisterVFX();
            }
            else
            {
                // 다음판 리셋
                _audioSource.Play();
                StartCoroutine(ResetBoard());

            }
        }
        IEnumerator ResetBoard()
        {

            // 타이머 이펙트 초기화
            ResetTimerEffects();

            // 타일 무너짐 애니메이션
            foreach (Transform tile in UIMiniTwo.GridParent)
            {
                //Vector3 randomOffset = new Vector3(
                //    Random.Range(-200f, 200f),
                //    Random.Range(-300f, -100f),
                //    0f
                //);

                //tile.DOLocalMove(tile.localPosition + randomOffset, 0.4f).SetEase(Ease.InBack);
                tile.DORotate(new Vector3(0, 0, Random.Range(-180f, 180f)), 0.4f);
                tile.DOScale(0f, 0.4f);
            }


            yield return new WaitForSeconds(0.6f);

            // 기존 타일 제거
            foreach (Transform tile in UIMiniTwo.GridParent)
            {
                Destroy(tile.gameObject);
            }

            yield return new WaitForSeconds(0.2f);

            foreach (GameObject vfx in VFXs)
            {
                Destroy(vfx);
            }
            UnRegisterVFX();

            StartNewRound();
        }

        void StartNewRound()
        {

            redCount = 0;
            timer = GameTime;

            fuseStarted = false; // 시계 아이콘 리셋
            ResetHearts();
            ResetGoals();
            UIMiniTwo.InitGame();

            TimerIcon.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }

        void ResetHearts()
        {
            foreach (GameObject heart in HeartPrefab)
            {
                heart.SetActive(false);
            }

            for (int i = 0; i < currentHealth; i++)
            {

                HeartPrefab[i].SetActive(true);
            }
        }

        void ResetGoals()
        {
            foreach (GameObject goal in GoalPrefab)
            {
                goal.SetActive(false);
            }
            for (int i = 0; i < successCount; i++)
            {
                GoalPrefab[i].SetActive(true);
            }
        }

        void TimerEffect()
        {
            TimerText.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            TimerText.transform.DOShakePosition(0.3f, 10f, 10, 90f, false, true).SetLoops(-1, LoopType.Restart);
            TimerIcon.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
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
