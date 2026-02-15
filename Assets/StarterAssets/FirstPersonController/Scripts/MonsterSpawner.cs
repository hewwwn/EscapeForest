using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int spawnCount = 30; // 생성할 몬스터 수

    private Terrain terrain;

    void Start()
    {
        terrain = Terrain.activeTerrain; // 현재 활성화된 Terrain 가져오기
        if (terrain == null)
        {
            Debug.LogError("활성화된 Terrain을 찾을 수 없습니다!");
            return;
        }

        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        Vector3 terrainSize = terrain.terrainData.size; // Terrain 크기 가져오기
        Vector3 terrainPosition = terrain.transform.position; // Terrain 위치 가져오기

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomPosition(terrainPosition, terrainSize);
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition(Vector3 terrainPosition, Vector3 terrainSize)
    {
        // X와 Z 좌표를 Terrain 내부로 제한
        float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
        float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

        // Raycast를 통해 정확한 높이 계산
        Vector3 rayOrigin = new Vector3(randomX, 100f, randomZ); // Ray 발사 시작점
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 200f))
        {
            return new Vector3(randomX, hit.point.y, randomZ);
        }

        // Raycast 실패 시 기본 높이를 반환
        return new Vector3(randomX, terrainPosition.y, randomZ); // Terrain 기본 높이
    }
}
