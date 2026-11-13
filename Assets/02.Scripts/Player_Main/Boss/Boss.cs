using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using DocumentGame;
using UnityEngine;
using UnityEngine.AI;

namespace CreateMap
{
    public class Boss : MonoBehaviour
    {
        public Transform player; // 추적할 플레이어
        public float moveSpeed = 3f; // AI의 이동 속도

        [SerializeField] NavMeshAgent _agent;

        [Header("스턴효과")]
        public bool IsStun;
        public GameObject StunEffect;
        public float StunTime;
        Animator _animator;
        float _currentStunTime;
        bool _isActive;
        Rigidbody2D _rb;
        SpriteRenderer _sr;


        void Start()
        {
            StunEffect.SetActive(false);
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();

            // _agent = GetComponent<NavMeshAgent>();

            _agent.updateRotation = false;
            _agent.updateUpAxis = false;

            _rb.isKinematic = false; // 물리적 상호작용을 받도록 설정

            IsStun = false;

            // 튜토리얼이 있으면 끝난 뒤에 추격 시작 (Awake 순서에 의존하지 않도록 Start에서 판단)
            DialogueManager dialogue = DialogueManager.Instance;
            if (dialogue != null && !dialogue.HasEnded)
            {
                _isActive = false;
                dialogue.OnDialogueEnded += StartBoss;
            }
            else
            {
                _isActive = true;
            }
        }

        public bool IsActive => _isActive;

        void Update()
        {
            if (!_isActive) return;

            if (!IsStun)
            {
                _agent.SetDestination(player.position);
                UpdateWalkVisual();
            } else
            {
                _agent.isStopped = true;
                StunEffect.SetActive(true);
                _animator.SetBool("Walk", false);
                _currentStunTime += Time.deltaTime;
                if (_currentStunTime >= StunTime)
                {
                    _agent.isStopped = false;
                    StunEffect.SetActive(false);
                    _currentStunTime = 0;
                    IsStun = false;
                }
            }
        }

        void OnDestroy()
        {
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.OnDialogueEnded -= StartBoss;
        }

        public void StartBoss()
        {
            _isActive = true;
        }

        // NavMeshAgent 이동 방향에 맞춰 걷기 애니메이션/좌우 반전 (기존 MoveTowardsPlayer는 호출되지 않아 항상 정지 모션이었음)
        void UpdateWalkVisual()
        {
            Vector3 velocity = _agent.velocity;
            bool walking = velocity.sqrMagnitude > 0.01f;
            _animator.SetBool("Walk", walking);

            if (velocity.x < -0.05f) _sr.flipX = true;
            else if (velocity.x > 0.05f) _sr.flipX = false;
        }

        public void Stun()
        {
            Player _player = player.GetComponent<Player>();
            StartButton startButton = player.GetComponent<PlayerGame>().StartButton;

            // 미니게임을 실제로 하는 중에만 막는다 (도망친 뒤 미니게임 자리에서도 기절시킬 수 있게)
            if (startButton.IsPlayingGame()) return;
            if (_player.StunItem > 0)
            {
                _player.UseStunItem();

                ProCamera2DShake.Instance.Shake(ProCamera2DShake.Instance.ShakePresets[0]);

                IsStun = true;
            }

            // 튜토리얼: 기절 버튼을 눌러 보면 "6시까지 버텨봐!"를 2초 보여 준 뒤 튜토리얼 종료
            if (DialogueManager.IsStep(TutorialStep.StunBoss))
            {
                DialogueManager.Instance.NextDialogue();
                StartCoroutine(Wait2Seconds());
            }
        }

        IEnumerator Wait2Seconds()
        {
            yield return new WaitForSeconds(2f);
            if (DialogueManager.Instance != null) DialogueManager.Instance.NextDialogue();
        }
    }
}
