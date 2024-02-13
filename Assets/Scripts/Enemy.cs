using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public float agentSpeed = 1f;
    public float health = 1f;
    public float damage = 1f;
    public NavMeshAgent agent;
    private Tower tower;
    public int maxHealth = 20;
    private int currentHealth;

    public void Start()
    {
        tower = FindObjectOfType<Tower>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(tower.transform.position);
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {

        //  agent.enabled = true;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die();

        }
    }

    void Die()
    {
        agent.enabled = false;
        // Destroy the enemy object
        gameObject.SetActive(false);
    }
}
