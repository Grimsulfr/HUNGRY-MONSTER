using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Componentes
    private Rigidbody2D rb;
    public Animator anim;
    private SpriteRenderer spriteRenderer;

    //Fisicas y Movimiento
    public float jump;
    private bool isGrounded;
    private bool isSliding = false;
    private bool isJumping = false;


    //Sistema de Vida
    public int maxHealth = 100;
    public int currentHealth;

    //Sistema de PowerUp e Invencibility
    public bool isInvincible=false;
    public float invincibilityDuration = 5f;
    public Coroutine invincibilityCoroutine;

    //Porcentaje de Daño
    [Range(0,100)] public int lowDamagePercent = 45;
    [Range(0,100)] public int midDamagePercent = 30;
    [Range(0,100)] public int highDamagePercent = 25;

    //Sistemas Extra de acomodar
    private bool isDead = false;
    private bool isPaused = false;

    //Identidad de Capas
    private int lowObstacleLayer;
    private int midObstacleLayer;
    private int highObstacleLayer;

    private int itemHealLayer;

    private int itemPowerUpLayer;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        lowObstacleLayer = LayerMask.NameToLayer("LowObstacle");
        if (lowObstacleLayer == -1) lowObstacleLayer = 7;

        midObstacleLayer = LayerMask.NameToLayer("MidObstacle");
        highObstacleLayer = LayerMask.NameToLayer("HighObstacle");
        itemHealLayer = LayerMask.NameToLayer("ItemHeal");
        itemPowerUpLayer = LayerMask.NameToLayer("ItemPowerUp");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        Time.timeScale = 1f;
    }

    //Sistema de Deteccion de Colision con suelo
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
         if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    //Detector de obstaculos e items
    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerObject = other.gameObject.layer;

        bool isRunning = isGrounded && !isSliding && !isJumping;

        if (layerObject == lowObstacleLayer && !isJumping)
        {
            Debug.Log("SE DETECTA IF DE BOMB!! :D");
            TakeDamagePercent(lowDamagePercent);
            Destroy(other.gameObject);
        }

        //Proyectiles
        else if (layerObject == midObstacleLayer && isRunning)
        {
            TakeDamagePercent(midDamagePercent);
            Destroy(other.gameObject);
        }
        //Kinsecto
        else if (layerObject == highObstacleLayer && isJumping)
        {
            TakeDamagePercent(highDamagePercent);
            Destroy(other.gameObject);
        }
        //Carnes y Draco
        else if (layerObject == itemHealLayer)
        {
            HealHealth(30);
            Destroy(other.gameObject);
        }
        else if (layerObject == itemPowerUpLayer)
        {
            ActivateInvincibility(invincibilityDuration);
            Destroy(other.gameObject);
        }
    }
    
    //Sistema e Daño y Derrota
    public void TakeDamagePercent(int percentage)
    {
        if (isDead || isInvincible) return;

        int damageAmount = Mathf.RoundToInt(maxHealth * (percentage/100f));
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    private void Die()
    {
        if(isDead) return;

        isDead = true;

        if (anim != null)
        {
            anim.SetBool("Sleep", true);
        }

        rb.linearVelocity = Vector2.zero;
    }

    //Sistema Curas y que no exceda max vida
    public void HealHealth(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    //Sistema de Invencibilidad
    public void ActivateInvincibility(float duration)
    {
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
        }

        invincibilityCoroutine = StartCoroutine(InvincibilityRoutine(duration));


    }
    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible=true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 0.8f);
        }

        yield return new WaitForSeconds(duration);

        isInvincible=false;

        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

    }


    //Sistema de Inputs
    public void OnJump(InputValue value)
    {
        if (isDead) return;

        if(isGrounded)
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
            isSliding = false;
            isJumping = true;

        }
    }

    public void OnCrouch(InputValue value)
    {
        if (isDead) return;

        if(isGrounded && value.isPressed)
        {
            anim.SetBool("Slide", true);
            isSliding = true;
        }
        else
        {
            anim.SetBool("Slide", false);
            isSliding = false;
        }
    }
    
    public void OnRestartAndPause(InputValue value)

    {
        if(!value.isPressed) return;

        if(isDead)
        {
            RestartGame();
        }

        else
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Level 0");
    }
}
