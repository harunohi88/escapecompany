using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UIElements;
using System;

namespace DocumentGame
{
    public class UI_MiniGame1 : MonoBehaviour
    {
        public static UI_MiniGame1 Instance;

        public Action OnCloseButtonClicked;

        public TextMeshProUGUI ComboText;

        public GameObject ResultPanel;
        public TextMeshProUGUI TotalScore;
        public TextMeshProUGUI MaxCombo;
        public TextMeshProUGUI PlayTime;
        public TextMeshProUGUI CorrectDocument;
        public TextMeshProUGUI FaultImportant;
        public TextMeshProUGUI FaultTrash;
        public GameObject CloseButton;
        public TextMeshProUGUI Fever;
        public GameObject FeverVFX;

        private void Awake()
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

        public void ActivateCombo()
        {
            ComboText.gameObject.SetActive(true);
        }

        public void InactivateCombo()
        {
            ComboText.gameObject.SetActive(false);
        }

        public void RefreshComboText(int combo)
        {
            ComboText.text = $"{combo}";

            ComboText.transform.DOScale(1.5f, 0.02f).OnComplete(() =>
            {
                ComboText.transform.DOScale(1f, 0.02f);
            });

            Color color = Color.Lerp(Color.white, Color.red, Mathf.Clamp01(combo / 10f));
            ComboText.DOColor(color, 0.2f);
        }

        public void ShowResult(int totalScore, int maxCombo, float playTime, int correctDocument, int faultImportant, int faultTrash)
        {
            ResultPanel.SetActive(true);
            InactivateCloseButton();


            // �ʱ�ȭ
            TotalScore.alpha = 0;
            MaxCombo.alpha = 0;
            PlayTime.alpha = 0;
            CorrectDocument.alpha = 0;
            FaultImportant.alpha = 0;
            FaultTrash.alpha = 0;

            // �ؽ�Ʈ ����
            TotalScore.text = $"���� ���� : {totalScore}";
            MaxCombo.text = $"�ִ� �޺� : {maxCombo}";
            PlayTime.text = $"���� �ð� : {playTime:F2}";
            CorrectDocument.text = $"�Ǹ��� �� ó�� : {correctDocument}";
            FaultImportant.text = $"���ƹ��� ���� ��� : {faultImportant}";
            FaultTrash.text = $"�����ϰ� ������ ������ : {faultTrash}";

            // �� �پ� ��Ÿ���� ȿ��
            Sequence sequence = DOTween.Sequence();
            sequence.Append(TotalScore.DOFade(1, 0.7f));
            sequence.Append(MaxCombo.DOFade(1, 0.7f));
            sequence.Append(PlayTime.DOFade(1, 0.7f));
            sequence.Append(CorrectDocument.DOFade(1, 0.7f));
            sequence.Append(FaultImportant.DOFade(1, 0.7f));
            sequence.Append(FaultTrash.DOFade(1, 0.7f)).OnComplete(() => ActivateCloseButton());
        }

        public void HideResult()
        {
            ResultPanel.SetActive(false);
            OnCloseButtonClicked?.Invoke();
        }

        public void ActivateCloseButton()
        {
            CloseButton.SetActive(true);
        }
        
        public void InactivateCloseButton()
        {
            CloseButton.SetActive(false);
        }

        public void ActivateFever()
        {
            Fever.gameObject.SetActive(true);
        }

        public void InactivateFever()
        {
            Fever.gameObject.SetActive(false);
        }
    }
}
