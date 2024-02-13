using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private GameObject target;
    private Rigidbody rb;
    public int damage = 4;
    private Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            // If the target is null (e.g., it was destroyed), deactivate the projectile
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "ground")
        {
            ParticleSystem blastEffect = ObjectPool.Instance.GetBlastFromPool();
            blastEffect.transform.position = transform.position;
            blastEffect.gameObject.SetActive(true);
            blastEffect.Play();
        }

        if (other.gameObject == target)
        {
            // Apply damage to the enemy
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            // Deactivate the projectile for reuse
            gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "ground")
        {
            gameObject.SetActive(false);
            return;

        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
    }
}