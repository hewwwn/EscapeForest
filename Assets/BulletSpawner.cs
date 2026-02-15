using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;    // 총알 프리팹
    public int spawnCount = 50;        // 생성할 총알 수
    public float spawnHeight = 0f;     // 총알이 생성될 높이 (terrain에서 높이를 계산하므로 기본값은 0)

    private Terrain terrain;           // Terrain
    private Vector3 terrainPosition;   // Terrain 위치
    private Vector3 terrainSize;       // Terrain 크기

    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain; // 활성화된 Terrain 가져오기

        if (terrain == null)
        {
            Debug.LogError("활성화된 Terrain을 찾을 수 없습니다!");
            return;
        }

        terrainPosition = terrain.transform.position;
        terrainSize = terrain.terrainData.size;

        SpawnBullets();
    }

    // 총알을 랜덤으로 생성
    void SpawnBullets()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // Terrain 내에서 랜덤한 X, Z 위치 생성
            float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
            float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

            // Raycast를 통해 해당 위치의 Y 좌표를 구함
            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);
            RaycastHit hit;

            // Raycast로 위치 확인 (Terrain 위로 생성)
            if (Physics.Raycast(spawnPosition + Vector3.up * 50, Vector3.down, out hit, 100f))
            {
                spawnPosition.y = hit.point.y;  // 맞은 위치의 Y값을 총알의 Y좌표로 설정
            }

            // 총알 생성
            Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
