using MiniGameTWo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CreateMap;
namespace DocumentGame
{
    public class StartButton : MonoBehaviour
    {
        public GameObject miniGame2Root;
        public GameTwoManager miniGame2Manager;

        public bool MiniGame1;
        public bool MiniGame2;

        [Header("도망 버튼 (미니게임 중단 -> 같은 자리에서 시작 버튼을 누르면 이어하기)")]
        [Tooltip("직접 만든 버튼을 넣으면 그걸 쓰고, 비워 두면 시작 버튼을 복제해 자동으로 만든다")]
        public Button FleeButton;
        // 시작 버튼과 같은 캔버스(900x1900, 가운데 기준) 기준 왼쪽 아래 조이스틱 자리.
        // 미니게임 캔버스가 위에 그려지므로 미니게임2 타일(아래 끝 y≈-521)과 겹치지 않게 둔다
        public Vector2 FleeButtonPosition = new Vector2(-300f, -680f);
        public float FleeButtonScale = 0.6f;
        // 노란 시작 버튼 이미지에 곱해지는 색 (노랑 x 이 색 = 초록 계열)
        public Color FleeButtonColor = new Color(0.45f, 1f, 0.45f);
        public string FleeButtonText = "도망!";

        enum ActiveGame
        {
            None,
            MiniGame1,
            MiniGame2,
        }

        ActiveGame _active;

        void Start()
        {
            // 씬에 StartButton이 두 개(시작/기절) 붙어 있으므로 플레이어가 참조하는 시작 버튼만 도망 버튼을 맡는다
            PlayerGame playerGame = FindAnyObjectByType<PlayerGame>();
            if (playerGame != null && playerGame.StartButton == this)
            {
                SetupFleeButton();
            }
        }

        void Update()
        {
            if (FleeButton == null) return;

            bool show = ShouldShowFleeButton();
            if (FleeButton.gameObject.activeSelf != show)
            {
                FleeButton.gameObject.SetActive(show);
            }
        }

        public void OnClickMiniGame1Start()
        {
            if (MiniGame1 == false) return;
            if (DocumentGameManager.Instance.NewGame())
            {
                _active = ActiveGame.MiniGame1;
            }

            // 대사창을 숨기고 '부장 기절' 안내를 미리 넣어 둔다. 미니게임1이 끝나면 DocumentGameManager가 보여 준다.
            if (!DialogueManager.IsStep(TutorialStep.PlayMiniGame1)) return;

            DialogueManager.Instance.DeActiveDialogueBox();
            DialogueManager.Instance.NextDialogue();
        }

        public void OnClickMiniGame2Start()
        {
            if (MiniGame2 == false) return;

            EnterMiniGameTwo();
            _active = ActiveGame.MiniGame2;

            // 미니게임2 중에는 대사창만 숨긴다. 다음 대사로 넘기는 건 GameTwoManager.GameOver에서 한다.
            if (!DialogueManager.IsStep(TutorialStep.PlayMiniGame2)) return;

            DialogueManager.Instance.DeActiveDialogueBox();
        }

        // 도망 버튼: 하던 미니게임을 중단(진행 상황 유지)하고 다시 움직일 수 있게 한다
        public void Flee()
        {
            switch (_active)
            {
            case ActiveGame.MiniGame1:
                DocumentGameManager.Instance.Suspend();
                break;
            case ActiveGame.MiniGame2:
                miniGame2Manager.Suspend();
                break;
            }
            _active = ActiveGame.None;
        }

        void EnterMiniGameTwo()
        {
            miniGame2Root.SetActive(true);
            miniGame2Manager.InitGame();
        }

        // 미니게임 자리(트리거) 안에 서 있는지
        public bool IsAnyGame()
        {
            return MiniGame1 || MiniGame2;
        }

        // 지금 미니게임을 실제로 플레이 중인지 (도망쳐서 중단한 상태는 false)
        public bool IsPlayingGame()
        {
            switch (_active)
            {
            case ActiveGame.MiniGame1:
                return DocumentGameManager.Instance != null && DocumentGameManager.Instance.IsPlaying;
            case ActiveGame.MiniGame2:
                return miniGame2Manager != null && miniGame2Manager.IsPlaying;
            }
            return false;
        }

        bool ShouldShowFleeButton()
        {
            // 튜토리얼 중에는 부장이 움직이지 않으므로 도망칠 필요가 없다 (튜토리얼 대사 흐름도 단순하게 유지)
            if (DialogueManager.Instance != null && !DialogueManager.Instance.HasEnded) return false;

            return IsPlayingGame();
        }

        void SetupFleeButton()
        {
            if (FleeButton == null)
            {
                FleeButton = CreateFleeButton();
            }
            FleeButton.onClick.AddListener(Flee);
            FleeButton.gameObject.SetActive(false);
        }

        // 시작 버튼을 복제해 같은 모양의 버튼을 만들고, 원래 클릭 이벤트(Play/미니게임 시작)는 끈다
        Button CreateFleeButton()
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            clone.name = "FleeButton";
            Destroy(clone.GetComponent<StartButton>());

            Button button = clone.GetComponent<Button>();
            for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
            {
                button.onClick.SetPersistentListenerState(i, UnityEventCallState.Off);
            }

            RectTransform rect = (RectTransform)clone.transform;
            rect.anchoredPosition = FleeButtonPosition;
            rect.localScale = Vector3.one * FleeButtonScale;

            Image image = clone.GetComponent<Image>();
            if (image != null) image.color = FleeButtonColor;

            GameObject labelObject = new GameObject("Label", typeof(RectTransform));
            labelObject.transform.SetParent(clone.transform, false);
            RectTransform labelRect = (RectTransform)labelObject.transform;
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            TextMeshProUGUI label = labelObject.AddComponent<TextMeshProUGUI>();
            label.text = FleeButtonText;
            label.alignment = TextAlignmentOptions.Center;
            label.fontSize = 110;
            label.fontStyle = FontStyles.Bold;
            label.color = Color.white;
            label.raycastTarget = false;
            // 기본 TMP 폰트에는 한글이 없으므로 결과창(한글)에 쓰는 폰트를 빌려 쓴다
            if (UI_MiniGame1.Instance != null && UI_MiniGame1.Instance.TotalScore != null)
            {
                label.font = UI_MiniGame1.Instance.TotalScore.font;
            }
            label.outlineWidth = 0.2f;
            label.outlineColor = Color.black;

            return button;
        }
    }
}
