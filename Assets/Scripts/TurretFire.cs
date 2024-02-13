using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurretFire : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject barragePrefab;
    public int initialPoolSize = 10;
    public float fireballCooldown = 5f;
    public float barrageCooldown = 10f;
    private float nextFireballTime = 0f;
    private float nextBarrageTime = 0f;
    public float attackRange = 30f;
    private List<GameObject> projectilePool;
    private GameObject currentTarget;
    void Start()
    {
        projectilePool = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            ExpandPool(fireballPrefab);
            ExpandPool(barragePrefab);
        }
    }

    void Update()
    {
        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange)
        {
            currentTarget = FindNearestEnemy();
        }
        // Cast the Fireball spell if it's off cooldown
        if (currentTarget != null && Time.time > nextFireballTime)
        {
            FireballSpell();
            nextFireballTime = Time.time + fireballCooldown;
        }

        // If there is a target, and the Barrage spell is off cooldown, cast it
        if (currentTarget != null && Time.time > nextBarrageTime)
        {
            BarrageSpell();
            nextBarrageTime = Time.time + barrageCooldown;
        }
    }

    void ExpandPool(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        projectilePool.Add(obj);
    }
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= attackRange)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
    GameObject GetFromPool(string type)
    {
        foreach (GameObject obj in projectilePool)
        {
            if (!obj.activeInHierarchy && obj.name.Contains(type))
            {
                return obj;
            }
        }

        // If no inactive object is found, expand the pool
        if (type == "Fireball")
        {
            ExpandPool(fireballPrefab);
        }
        else if (type == "Barrage")
        {
            ExpandPool(barragePrefab);
        }

        return projectilePool[projectilePool.Count - 1];
    }

    void FireballSpell()
    {
        // Find a random enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            GameObject target = enemies[Random.Range(0, enemies.Length)];

            // Get a Fireball projectile from the pool and set its target
            GameObject fireball = GetFromPool("Fireball");
            fireball.transform.position = transform.position;
            fireball.SetActive(true);

            Projectile projectile = fireball.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetTarget(target);
            }
        }
    }

    void BarrageSpell()
    {
        // Find all visible enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Check if the enemy is visible on the screen
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
            bool isVisible = screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;

            if (isVisible)
            {
                // Get a Barrage projectile from the pool and set its target
                GameObject barrage = GetFromPool("Barrage");
                barrage.transform.position = transform.position;
                barrage.SetActive(true);

                Projectile projectile = barrage.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.SetTarget(enemy);
                }
            }
        }
    }
}