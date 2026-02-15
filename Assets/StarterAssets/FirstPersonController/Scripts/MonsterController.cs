using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public Transform player; // 플레이어를 드래그 앤 드롭으로 연결
    private NavMeshAgent agent;
    private Animator animator; // 애니메이터 컴포넌트 추가

    [Header("Monster Settings")]
    public float speed = 3.5f; // 괴물의 속도 조절 변수
    public float stoppingDistance = 0.5f; // 플레이어와의 최소 거리
    public float detectionRadius = 10f; // 플레이어 감지 반경
    public int damage = 20; // 플레이어에게 줄 데미지

    [Header("Health Settings")]
    public int maxHealth = 100; // 최대 체력
    private int currentHealth; // 현재 체력

    [Header("Collision Settings")]
    public float attackCooldown = 1f; // 공격 쿨타임 (초)
    private float lastAttackTime = 0f; // 마지막 공격 시간 기록

    [Header("Audio Settings")]
    public AudioClip footstepSound; // 발자국 소리
    public AudioClip attackSound;   // 공격 소리
    public float footstepInterval = 0.5f; // 발자국 소리 간격 (초)
    private AudioSource audioSource;
    private float footstepTimer = 0f;

    private bool isPlayerDetected = false; // 플레이어가 감지됐는지 여부

    void Start()
    {
        // NavMeshAgent 컴포넌트 가져오기
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기

        // 체력 초기화
        currentHealth = maxHealth;

        // Rigidbody 컴포넌트 추가 및 설정
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>(); // Rigidbody가 없으면 추가
        }

        if (player == null)
        {
            Debug.LogError("Player is not assigned! Please drag and drop the player object in the Inspector.");
        }

        // 초기 속도와 스탑핑 거리 설정
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;

        // AudioSource 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 초기 애니메이션 상태: Idle 상태로 설정
        animator.SetBool("IsChasing", false);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Monster took damage! Current health: " + currentHealth); // 체력 변화 확인


        // 체력이 0 이하로 떨어지면 좀비가 죽음
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 좀비 죽는 함수
    private void Die()
    {
        Debug.Log("Zombie died!");

        // 죽는 애니메이션 트리거
        animator.SetTrigger("IsDead"); // "IsDead"는 애니메이터에서 설정한 파라미터

        // NavMeshAgent를 비활성화하여 이동 멈추기
        agent.isStopped = true;  // 이동을 멈춤
        agent.enabled = false;   // NavMeshAgent 비활성화

        // 죽음 애니메이션이 끝나면 게임 오브젝트 삭제 (또는 사망 후 다른 행동)
        Destroy(gameObject, 4f); // 2초 후에 삭제 (애니메이션 길이에 맞게 조정)
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어가 감지된 경우에만 추적
            if (isPlayerDetected)
            {
                // 플레이어 위치를 목적지로 설정
                agent.SetDestination(player.position);

                // 발자국 소리 재생
                if (IsMoving())
                {
                    footstepTimer += Time.deltaTime;
                    if (footstepTimer >= footstepInterval)
                    {
                        PlayFootstepSound();
                        footstepTimer = 0f;
                    }
                }

                // 몬스터가 플레이어를 추적할 때 애니메이션 변경
                animator.SetBool("IsChasing", true);
            }
            else
            {
                // 플레이어가 감지 범위 내에 들어왔는지 확인
                if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
                {
                    isPlayerDetected = true;
                    Debug.Log("Player detected! Monster starts chasing.");
                }

                // 애니메이션 상태를 Idle로 변경
                animator.SetBool("IsChasing", false);
            }
        }
    }

    private bool IsMoving()
    {
        // NavMeshAgent의 속도로 이동 여부 확인
        return agent.velocity.magnitude > 0.1f;
    }

    private void PlayFootstepSound()
    {
        if (footstepSound != null)
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }

    private void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.transform.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Debug.Log("Monster has attacked the player!");
                PlayAttackSound();

                // 마지막 공격 시간 갱신
                lastAttackTime = Time.time;

                // 플레이어의 체력을 감소
                FirstPersonController playerController = collision.transform.GetComponent<FirstPersonController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                }
            }
            else
            {
                Debug.Log("Attack on cooldown.");
            }
        }
    }

}
