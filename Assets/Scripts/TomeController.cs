using TMPro;
using UnityEngine;

public class TomeController : MonoBehaviour
{
    public Transform player;
    public Transform floatingObject;
    public GameObject projectilePrefab;
    public float distanceFromPlayer = 2f;
    public float hoverHeight = 0.5f;
    public float hoverSpeed = 2f;
    public float transitionSpeed = 2f;
    public float projectileSpeed = 5f;
    public float shootingCooldown = 2f;
    public float pageBoost = 0.1f;

    private Vector3 offset;
    private Vector3 targetDirection; // Target direction for the floating object to face
    private Vector3 currentDirection; // Current direction the floating object is moving in
    private float lastFireTime = -2f;

    public TextMeshProUGUI AttackSpeedText;

    [Header("Audio Settings")]
    public AudioClip shotSound;
    private AudioSource audioSource;

    void Start()
    {
        // Calculate the initial offset of the floating object from the player
        offset = floatingObject.position - player.position;
        targetDirection = offset.normalized;
        currentDirection = targetDirection;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 directionToMouse = mousePosition - player.position;

        // Create a vector that only keeps the X or Y direction, ensuring movement is only horizontal or vertical
        Vector3 newDirection = Vector3.zero;

        if (Mathf.Abs(directionToMouse.x) > Mathf.Abs(directionToMouse.y))
        {
            // Move horizontally
            newDirection.x = directionToMouse.x > 0 ? 1 : -1;
        }
        else
        {
            // Move vertically
            newDirection.y = directionToMouse.y > 0 ? 1 : -1;
        }

        if (newDirection != currentDirection)
        {
            targetDirection = newDirection;
        }

        // Smoothly transition towards the new direction
        currentDirection = Vector3.Lerp(currentDirection, targetDirection, transitionSpeed * Time.deltaTime);

        // Apply movement in the determined direction, at a fixed distance from the player
        floatingObject.position = player.position + currentDirection * distanceFromPlayer;

        // Apply hovering effect
        float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        floatingObject.position = new Vector3(floatingObject.position.x, floatingObject.position.y + hover, floatingObject.position.z);

        // Handle shooting logic
        if (Input.GetMouseButton(0) && Time.time - lastFireTime >= shootingCooldown)
        {
            ShootProjectile(directionToMouse);
            lastFireTime = Time.time;
        }
    }

    void ShootProjectile(Vector3 direction)
    {
        // Instantiate a new projectile
        GameObject projectile = Instantiate(projectilePrefab, floatingObject.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        audioSource.clip = shotSound;
        audioSource.Play();

        // Normalize the direction and apply velocity to the projectile's Rigidbody2D
        rb.velocity = direction.normalized * projectileSpeed;
    }

    public void IncreaseFireRate()
    {
        if (shootingCooldown > pageBoost) {
            shootingCooldown -= pageBoost;
        } else shootingCooldown = 0f;

        if (shootingCooldown != 1)
        {
            AttackSpeedText.text = "Attack Cooldown: " + shootingCooldown.ToString("F2") + " Seconds";
        }
        else
        {
            AttackSpeedText.text = "Attack Cooldown: " + shootingCooldown.ToString("F2") + " Second";
        }
    }
}
