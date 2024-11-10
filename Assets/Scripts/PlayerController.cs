using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject tome;
    public int maxHealth = 3;
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private LayerMask collectibleMask;
    private TomeController tomeController;
    private int health;

    public List<HeartScript> hearts;
    public float Score;
    public TextMeshProUGUI ScoreText;
    public GameObject HighScoreText;
    public bool dead;

    public Animator RetryAnim;

    public GameObject DeathScreen;

    private void Start()
    {
        Score = 0;
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator.Play("Anim_Witch_Floating");
        collectibleMask = 1 << LayerMask.NameToLayer("Collectible");
        tomeController = tome.GetComponent<TomeController>();
    }

    private void Update()
    {
        if (!dead)
        {
            Score += Time.deltaTime;
            ScoreText.text = "Score: " + Score.ToString("F0");
        }
        // Get movement input for both X and Y axes
        float moveInputX = Input.GetAxisRaw("Horizontal");  // Left/Right (A/D or Arrow Keys)
        float moveInputY = Input.GetAxisRaw("Vertical");    // Up/Down (W/S or Arrow Keys)

        // Apply movement in both X and Y axes using Rigidbody2D velocity
        rb.velocity = new Vector2(moveInputX, moveInputY).normalized * moveSpeed;

        UpdateDirectionAndAnimation(moveInputX);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((collectibleMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.gameObject.CompareTag("Page") && tomeController != null)
            {
                tomeController.IncreaseFireRate();
            }
            if (other.gameObject.CompareTag("Heart"))
            {
                AddHealth();
            }
            Destroy(other.gameObject);
        }
    }

    private void UpdateDirectionAndAnimation(float moveInputX)
    {
        // Flip the sprite based on the direction
        if (moveInputX != 0)
        {
            spriteRenderer.flipX = moveInputX < 0;
        }
    }

    public void TakeDamage()
    {
        health--;
        animator.Play("Anim_Witch_Damage", -1, 0f);
        UpdateHearts();

        StartCoroutine(WaitForDamageAnimation());
    }

    public void AddHealth()
    {
        if (health < maxHealth)
        {
            health++;
        }
        UpdateHearts();

        StartCoroutine(WaitForDamageAnimation());
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < health)
            {
                hearts[i].FullHeart.SetActive(true);
                hearts[i].EmptyHeart.SetActive(false);
            }
            else
            {
                hearts[i].FullHeart.SetActive(false);
                hearts[i].EmptyHeart.SetActive(true);
            }
        }
    }


    private IEnumerator WaitForDamageAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Handle the player's death (e.g., play animation, show death screen, etc.)

        if (PlayerPrefs.GetInt("HighScore", 0) < Score)
        {
            PlayerPrefs.SetInt("HighScore", Mathf.RoundToInt(Score));
        }

        //Destroy(gameObject);

        DeathScreen.SetActive(true);
        Time.timeScale = 0f;
        RetryAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        dead = true;
        HighScoreText.SetActive(false);

        //SceneManager.LoadScene("StartScene");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameLevel");
    }
}
