using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ParticleSystemJobs;
public class ObjectPool : MonoBehaviour
{
     public static ObjectPool Instance { get; private set; }

    public GameObject defaultEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject bigSlowEnemyPrefab;
    public int initialPoolSize = 10;

    private Dictionary<string, List<GameObject>> enemyPools;
    public ParticleSystem blastEffectPrefab;
    public int initialBlastPoolSize = 20;

    private List<ParticleSystem> blastEffectPool;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    void Start()
    {
        enemyPools = new Dictionary<string, List<GameObject>>
        {
            { "default", new List<GameObject>() },
            { "fast", new List<GameObject>() },
            { "bigSlow", new List<GameObject>() }
        };

        for (int i = 0; i < initialPoolSize; i++)
        {
            ExpandPool("default", defaultEnemyPrefab);
            ExpandPool("fast", fastEnemyPrefab);
            ExpandPool("bigSlow", bigSlowEnemyPrefab);
        }
        blastEffectPool = new List<ParticleSystem>();

        for (int i = 0; i < initialBlastPoolSize; i++)
        {
            ExpandBlastPool();
        }
    }

    private void ExpandPool(string type, GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab);
        enemy.SetActive(false);
        enemyPools[type].Add(enemy);
    }

    public GameObject GetEnemyFromPool(string type)
    {
        foreach (GameObject enemy in enemyPools[type])
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        // If no inactive enemy is found, expand the pool
        switch (type)
        {
            case "default":
                ExpandPool(type, defaultEnemyPrefab);
                break;
            case "fast":
                ExpandPool(type, fastEnemyPrefab);
                break;
            case "bigSlow":
                ExpandPool(type, bigSlowEnemyPrefab);
                break;
        }

        return enemyPools[type][enemyPools[type].Count - 1];
    }
     void ExpandBlastPool()
    {
        ParticleSystem obj = Instantiate(blastEffectPrefab);
        obj.gameObject.SetActive(false);
        blastEffectPool.Add(obj);
    }

    public ParticleSystem GetBlastFromPool()
    {
        foreach (ParticleSystem obj in blastEffectPool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        // If no inactive object is found, expand the pool
        ExpandBlastPool();
        return blastEffectPool[blastEffectPool.Count - 1];
    }
}