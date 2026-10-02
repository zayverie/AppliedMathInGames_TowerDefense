using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform sharedPoint; 

    [Header("Spawn Limits")]
    public int maxEnemiesToSpawn = 200;
    public int totalEnemiesSpawned = 0;
    public bool hasFinishedSpawning = false;

    [Header("Quadratic Spawn Points")]
    public Transform quadSpawnPoint; 
    public Transform controlQuadPoint; 
    public float quadSpawnInterval = 0.1f; 
    public float nextQuadSpawnTime; 
    
    [Header("Cubic Spawn Points")]
    public Transform cubicSpawnPoint; 
    public Transform controlCubicPoint1; 
    public Transform controlCubicPoint2; 
    public float cubicSpawnInterval = 0.1f; 
    public float nextCubicSpawnTime; 

    void Start()
    {
        nextQuadSpawnTime = Time.time + quadSpawnInterval; 
        nextCubicSpawnTime = Time.time + cubicSpawnInterval; 
    }

    void Update()
    {
        if (totalEnemiesSpawned >= maxEnemiesToSpawn)
        {
            hasFinishedSpawning = true;
            return;
        }

        if (Time.time >= nextQuadSpawnTime) 
        {
            QuadraticSpawn(); 
            nextQuadSpawnTime = Time.time + quadSpawnInterval; 
        }

        if (Time.time >= nextCubicSpawnTime) 
        {
            CubicSpawn(); 
            nextCubicSpawnTime = Time.time + cubicSpawnInterval; 
        }
    }

    public void QuadraticSpawn() 
    {
        if (totalEnemiesSpawned >= maxEnemiesToSpawn) return;

        if (enemyPrefab != null && quadSpawnPoint != null && controlQuadPoint != null && sharedPoint != null) 
        {
            GameObject enemy = Instantiate(enemyPrefab, quadSpawnPoint.position, quadSpawnPoint.rotation); 
            Enemy enemyScript = enemy.GetComponent<Enemy>(); 

            enemyScript.curveType = Enemy.CurveType.Quadratic; 
            enemyScript.initialPosition = quadSpawnPoint.position; 
            enemyScript.p1 = controlQuadPoint.position; 
            enemyScript.p2 = sharedPoint.position; 

            totalEnemiesSpawned++;
        }
    }

    public void CubicSpawn() 
    {
        if (totalEnemiesSpawned >= maxEnemiesToSpawn) return;

        if (enemyPrefab != null && cubicSpawnPoint != null && controlCubicPoint1 != null && controlCubicPoint2 != null && sharedPoint != null) 
        {
            GameObject enemy = Instantiate(enemyPrefab, cubicSpawnPoint.position, cubicSpawnPoint.rotation); 
            Enemy enemyScript = enemy.GetComponent<Enemy>(); 

            enemyScript.curveType = Enemy.CurveType.Cubic; 
            enemyScript.initialPosition = cubicSpawnPoint.position; 
            enemyScript.p1 = controlCubicPoint1.position; 
            enemyScript.p2 = controlCubicPoint2.position; 
            enemyScript.p3 = sharedPoint.position; 

            totalEnemiesSpawned++;
        }
    }
}