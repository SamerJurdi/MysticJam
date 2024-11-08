using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 10;
    public float moveSpeed = 3f;
    public int damageToPlayer = 1;
    public float attackCooldown = 1f; // Time between damage ticks while colliding with the player
    private float attackTimer = 0f; // Timer to handle damage cooldown

    private Transform player;
    private PlayerController playerController;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        FollowPlayer();

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackTimer <= 0f)
        {
            playerController.TakeDamage(damageToPlayer);
            attackTimer = attackCooldown;
        }
    }

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
