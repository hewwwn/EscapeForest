using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;  // SceneManager를 사용하기 위한 네임스페이스
using System.Collections;

public class PlayerItemController : MonoBehaviour
{
    private RaycastHit hit;
    private Ray ray;

    public float getDistance = 2f;

    int keysCollected = 0;
    int bulletHave = 0;
    public GameObject keyUI;
    public GameObject bulletUI;
    public GameObject actionUI;

    public GameObject bulletPrefab;
    bool canShoot = true;
    float coolTime;
    public float coolTimeMax = 5f;

    public ParticleSystem gunFire;
    public AudioClip gunSound;
    public AudioSource source;
    public Light fireLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireLight.enabled = false;
    }

    void ObjectHit()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Key" || hit.collider.gameObject.tag == "Bullet")
            {
                if (Vector3.Distance(hit.collider.transform.position, transform.position) < getDistance)
                {
                    actionUI.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GetItem(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    void GetItem(GameObject item)
    {
        if (item.tag == "Key" && keysCollected < 5)  // 열쇠는 최대 5개까지
        {
            keysCollected++;
            Destroy(item);
        }
        if (item.tag == "Bullet" && bulletHave < 5)  // 총알은 최대 5개까지
        {
            bulletHave++;
            Destroy(item);
        }
    }

    private IEnumerator MuzzleFlash()
    {
        fireLight.enabled = true;
        yield return new WaitForSeconds(0.05f); // 불빛 지속 시간
        fireLight.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        actionUI.SetActive(false);
        ObjectHit();
 
        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot && bulletHave != 0)
            {
                gunFire.Play(); // 화염 이펙트
                source.PlayOneShot(gunSound); // 발사 소리
                StartCoroutine(MuzzleFlash()); // 화염 불빛
                bulletHave--;
                ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        // 좀비에 맞았을 때 체력 감소
                        MonsterController monsterController = hit.collider.gameObject.GetComponent<MonsterController>();
                        if (monsterController != null)
                        {
                            Debug.Log("Shooting the monster!"); // 총알이 맞은 몬스터 확인

                            monsterController.TakeDamage(100); // 10은 데미지 값 (원하는 데미지로 조정 가능)
                        }
                        Debug.Log("hit");
                    }
                    else
                    {
                        Debug.Log("not hit");
                    }
                }
                canShoot = false;
            }
        }

        if (!canShoot)
        {
            coolTime -= Time.deltaTime;
            if (coolTime < 0)
            {
                coolTime = 1f;
                canShoot = true;
            }
        }

        keyUI.GetComponent<TextMeshProUGUI>().text = keysCollected + "/5";
        bulletUI.GetComponent<TextMeshProUGUI>().text = bulletHave + "/5";



        // 열쇠가 5개 모이면 ClearScene으로 이동
        if (keysCollected >= 5)
        {
            GameClear();
        }
    }
    public GameObject gameClearUI; // Game Clear UI 오브젝트
    private void GameClear()
    {
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true); // Game Clear UI 활성화
        }

        // 마우스 커서 활성화
        Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
        Cursor.visible = true; // 마우스 커서를 보이게 설정

        Time.timeScale = 0f; // 게임 멈춤
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // 게임 시간 복구
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        Cursor.visible = false; // 마우스 커서를 숨김
        Application.Quit(); // 애플리케이션 종료

        // Unity 에디터에서 실행 중이라면 아래 로그를 확인할 수 있습니다.
        Debug.Log("게임 종료");
    }
}
