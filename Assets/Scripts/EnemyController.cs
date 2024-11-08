using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Handle the enemy's death (e.g., play an animation, destroy the enemy)
        Destroy(gameObject);
    }
}
