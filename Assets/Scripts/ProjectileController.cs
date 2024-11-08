using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float lifetime = 5f;
    private float timeAlive;
    public int damage = 5;
    public LayerMask collisionMask; // Which layers the projectile will interact with
    private EnemyController enemy;

    private void Start()
    {
        timeAlive = 0f;
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((collisionMask.value & (1 << other.gameObject.layer)) > 0)
        {
            // Handle specific trigger events here (e.g., if it hits a trigger zone or certain objects)
            Destroy(gameObject);
            enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
