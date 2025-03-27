using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTwoManager : MonoBehaviour
{
    public UIMiniTwo ui;
    public TextMeshProUGUI timerText;
    public float gameTime = 10f;

    [Header("체력시스템")]
    public Transform heartParent;
    public GameObject heartPrefab;

    private List<Animator> heartAnimators = new List<Animator>();
    private int currentHealth = 3;

    private float timer;
    private int redCount;

    void Start()
    {
        StartNewRound();
        //timer = gameTime;
        //ui.InitGame();  // 게임 시작
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer);

        if (timer <= 0)
        {
            Debug.Log("실패!");
            enabled = false;
        }
    }

    public void RegisterTile(bool isRed)
    {
        if (isRed) redCount++;
    }

    public void RedCleared()
    {
        redCount--;
        if (redCount <= 0)
        {
            Debug.Log("성공!");
            enabled = false;
        }
    }

    public void OnwrongClick()
    {
        if (currentHealth <= 0)
        {
            // 이미 체력이 0인 상태에서 잘못 누름 → 즉시 게임 종료 처리
            Debug.Log("게임 종료: 기회 소진");
            enabled = false;
            return;
        }

        // 하트 감소
        currentHealth--;

        // 하트 애니메이션 (SetTrigger)
        heartAnimators[currentHealth].SetTrigger("Lose");

        if (currentHealth <= 0)
        {
            // 하트 전부 소진 → 게임 종료 처리
            Debug.Log("하트 0: 게임 종료");
            enabled = false;
        }
        else
        {
            // 하트 남아 있음 → 다음 판으로 리셋
            //StartCoroutine(ResetBoard());
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
        ui.InitGame();
    }

    void ResetHearts()
    {
        foreach (Transform child in heartParent)
        {
            //Destroy(child.gameObject);
            gameObject.SetActive(false);
        }
        heartAnimators.Clear();

        for (int i = 0; i < 3; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartParent);
            Animator anim = heart.GetComponent<Animator>();
            heartAnimators.Add(anim);

            if (i >= currentHealth)
            {
                anim.SetTrigger("Lose");
            }
        }
    }
}
