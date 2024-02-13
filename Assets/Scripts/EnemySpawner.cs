using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public ObjectPool objectPool; // Replace 'objectPool' with your ObjectPool object

    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;
    private float timeElapsed = 0f;
    private Coroutine enemyEnableCoroutine;
   
    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // Increase spawn rate every minute
        if (timeElapsed > 60f)
        {
            spawnRate -= 0.5f;
            timeElapsed = 0f;
        }

        // Ensure spawn rate doesn't go below 1 second
        spawnRate = Mathf.Max(spawnRate, 1f);

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnEnemy()
    {
        // Randomly select an enemy type to spawn
        int enemyType = Random.Range(0, 3);
        string enemyTypeKey;

        switch (enemyType)
        {
            case 0:
                enemyTypeKey = "default";
                break;
            case 1:
                enemyTypeKey = "fast";
                break;
            case 2:
                enemyTypeKey = "bigSlow";
                break;
            default:
                enemyTypeKey = "default";
                break;
        }

        // Get an enemy from the pool
        GameObject enemy = objectPool.GetEnemyFromPool(enemyTypeKey); // Replace 'objectPool' with your ObjectPool object
                                                                      // Replace 'tower' with your tower object
        Vector3 unitCirclepoint = Random.insideUnitCircle.normalized;
        Vector3 randomDirection = new Vector3(unitCirclepoint.x, 0, unitCirclepoint.y); // Keep the direction on the XZ plane

        // Multiply the direction by the radius to get the spawn position
        float radius = 80f; // Set this to the desired radius
        Vector3 spawnPosition = transform.position + (randomDirection * radius);

        enemy.transform.position = spawnPosition;
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.health = enemyScript.maxHealth;
        if (enemyScript.agent) enemyScript.agent.enabled = true;
        enemy.SetActive(true);
      
    }

}