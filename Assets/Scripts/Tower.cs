using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBar;
    public GameObject gameOverPanel;
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
    void Die()
    {
        gameOverPanel?.SetActive(true);
        Time.timeScale = 0;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.damage);
            enemy.TakeDamage(10);
        }
    }
}
