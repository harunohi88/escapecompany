using UnityEngine;

namespace CreateMap
{
    public class Boss : MonoBehaviour
    {
        public Transform player; // 추적할 플레이어
        public float moveSpeed = 3f; // AI의 이동 속도


        [Header("스턴효과")]
        public bool IsStun;
        public GameObject StunEffect;
        public float StunTime;
        Animator _animator;
        float _currentStunTime;
        Rigidbody2D _rb;
        SpriteRenderer _sr;
        void Start()
        {
            StunEffect.SetActive(false);
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
            _rb.isKinematic = false; // 물리적 상호작용을 받도록 설정
        }

        void Update()
        {

            if (!IsStun)
                MoveTowardsPlayer();
            else
            {
                StunEffect.SetActive(true);
                _animator.SetBool("Walk", false);
                _currentStunTime += Time.deltaTime;
                if (_currentStunTime >= StunTime)
                {
                    StunEffect.SetActive(false);
                    _currentStunTime = 0;
                    IsStun = false;
                }
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.GameOver();
            }
        }

        void MoveTowardsPlayer()
        {
            // 플레이어의 위치로 목표 설정
            Vector3 targetPosition = player.position;

            // 타일맵에 맞춰 AI가 이동
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (direction.x < 0)
            {
                _sr.flipX = true;
            } else
            {
                _sr.flipX = false;
            }

            // 충돌을 피하면서 이동
            _rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
            _animator.SetBool("Walk", true);

        }

        public void Stun()
        {
            Player _player = player.GetComponent<Player>();
            if (_player.StunItem > 0)
            {
                _player.UseStunItem();
                IsStun = true;
            }
        }
    }
}
