using DG.Tweening;
using System.Collections;

//using static UnityEditor.Rendering.CoreEditorDrawer<TData>;
using TMPro;
using UnityEngine;

namespace MiniGameTWo
{
    public class GameTwoManager : MonoBehaviour
    {
        public UIMiniTwo ui;
        public TextMeshProUGUI timerText;
        public float gameTime = 10f;
        private int successCount = 0;
        private const int maxSuccess = 3;

        [Header("체력시스템")]
        public GameObject[] heartPrefab;
        public GameObject[] goalPrefab;
        public GameObject bombPrefab;
        public GameObject UIPrefab;
        private bool fuseStarted = false;
        private bool isGameOver = false;


        private int currentHealth = 3;

        private float timer;
        private int redCount;

        void Start()
        {
            StartNewRound();
            //timer = gameTime;
            //ui.InitGame();  // 게임 시작
            bombPrefab.SetActive(false);
            //Debug.Log("bomb 끔.");
        }

        void Update()
        {
            timer -= Time.deltaTime;
            timerText.text = $"{Mathf.Ceil(timer)}";

            if (!fuseStarted && timer < 4f)
            {
                //Debug.Log("폭탄 발동!");
                bombPrefab.SetActive(true);
                fuseStarted = true;
                bombPrefab.GetComponent<Animator>().Play("BombSystem");
            }
            if (timer <= 0)
            {
                Debug.Log("실패!");
                isGameOver = true;
                UIPrefab.SetActive(false);
                enabled = false;
            }
        }

        public void RegisterTile(bool isRed)
        {
            if (isRed) redCount++;
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
                if (successCount <= goalPrefab.Length)
                {
                    goalPrefab[successCount - 1].SetActive(true);
                }

                if (successCount >= maxSuccess)
                {
                    Debug.Log("게임 클리어!");
                    isGameOver = true;
                    UIPrefab.SetActive(false);
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
                enabled = false;

                return;
            }


            Debug.Log($"{currentHealth}");
            // 하트 애니메이션 (SetTrigger)
            heartPrefab[currentHealth - 1].GetComponent<Animator>().Play("heart");
            // 하트 감소
            currentHealth--;
            if (currentHealth <= 0)
            {
                // 하트 전부 소진 → 게임 종료 처리
                Debug.Log("하트 0: 게임 종료");
                enabled = false;
                isGameOver = true;
                UIPrefab.SetActive(false);
            }
            else
            {
                // 다음판 리셋
                StartCoroutine(ResetBoard());
            }
        }
        IEnumerator ResetBoard()
        {
            // 타일 무너짐 애니메이션
            foreach (Transform tile in ui.gridParent)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-200f, 200f),
                    Random.Range(-300f, -100f),
                    0f
                );

                tile.DOLocalMove(tile.localPosition + randomOffset, 0.4f).SetEase(Ease.InBack);
                tile.DORotate(new Vector3(0, 0, Random.Range(-180f, 180f)), 0.4f);
                tile.DOScale(0f, 0.4f);
            }


            yield return new WaitForSeconds(0.6f);

            // 기존 타일 제거
            foreach (Transform tile in ui.gridParent)
            {
                Destroy(tile.gameObject);
            }

            yield return new WaitForSeconds(0.2f);

            StartNewRound();
        }

        void StartNewRound()
        {
            timer = gameTime;
            ResetHearts();
            ResetGoals();
            ui.InitGame();
        }

        void ResetHearts()
        {
            foreach (GameObject heart in heartPrefab)
            {
                heart.SetActive(false);
            }

            for (int i = 0; i < currentHealth; i++)
            {

                heartPrefab[i].SetActive(true);
            }
        }

        void ResetGoals()
        {
            foreach (GameObject goal in goalPrefab)
            {
                goal.SetActive(false);
            }
            for (int i = 0; i < successCount; i++)
            {
                goalPrefab[i].SetActive(true);
            }
        }
    }
}

