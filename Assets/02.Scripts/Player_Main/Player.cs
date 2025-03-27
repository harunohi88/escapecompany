using UnityEngine;

namespace CreateMap
{
    public class Player : MonoBehaviour
    {
        public float MoveSpeed = 5f; // 이동 속도
        Animator _animator;

        bool _isPlay;
        Vector2 _movement;
        Rigidbody2D _rigidbody2D;
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>(); // Rigidbody2D 가져오기
            _animator = GetComponent<Animator>(); // Animator 가져오기
        }

        void Update()
        {
            if (!_isPlay)
            {
                // 입력값 받기
                _movement.x = Input.GetAxisRaw("Horizontal");
                _movement.y = Input.GetAxisRaw("Vertical");
            }

            // 애니메이션 처리
            if (_movement.x != 0 || _movement.y != 0) // 이동 중
            {
                _animator.SetBool("Walk", true);

                // 좌우 반전 (오른쪽이 기본 방향)
                if (_movement.x > 0)
                    transform.localScale = new Vector3(1, 1, 1);
                else
                    transform.localScale = new Vector3(-1, 1, 1);
            } else // 정지 상태
            {
                _animator.SetBool("Walk", false);
                _animator.Play("Player_Idle");
            }

            if (Input.GetKey(KeyCode.Space))
            {
                _animator.SetBool("Play", true);
            }
        }

        void FixedUpdate()
        {
            // Rigidbody를 이용한 이동 처리
            _rigidbody2D.linearVelocity = _movement.normalized * MoveSpeed;
        }
    }
}
