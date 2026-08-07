using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Emisor de evento para UI
    public event Action<int, int> OnHealthChanged;
    public event Action<bool> OnPauseChanged;


    //Componentes
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    //Fisicas y Movimiento
    public State currentState;
    public RunningState runningState;
    public JumpingState jumpingState;
    public SlidingState slidingState;
    public DeadState deadState;

    //Sistema de Vida
    public float jump;
    public int maxHealth = 100;
    public int currentHealth;

    //Sistema de PowerUp e Invencibility
    public bool isInvincible = false;
    public float invincibilityDuration = 5f;
    public Coroutine invincibilityCoroutine;

    //Porcentaje de Daño
    [Range(0,100)] public int lowDamagePercent = 45;
    [Range(0,100)] public int midDamagePercent = 30;
    [Range(0,100)] public int highDamagePercent = 25;

    //Sistemas Extra de acomodar
    private bool isPaused = false;

    //Identidad de Capas
    private int lowObstacleLayer;
    private int midObstacleLayer;
    private int highObstacleLayer;
    private int itemHealLayer;
    private int itemPowerUpLayer;
    private int groundLayer;
    private int isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (anim == null) anim = GetComponent<Animator>();

        runningState = GetComponent<RunningState>() ?? gameObject.AddComponent<RunningState>();
        jumpingState = GetComponent<JumpingState>() ?? gameObject.AddComponent<JumpingState>();
        slidingState = GetComponent<SlidingState>() ?? gameObject.AddComponent<SlidingState>();
        deadState = GetComponent<DeadState>() ?? gameObject.AddComponent<DeadState>();

        runningState.Init(this);
        jumpingState.Init(this);
        slidingState.Init(this);
        deadState.Init(this);

        lowObstacleLayer = LayerMask.NameToLayer("LowObstacle");
        if (lowObstacleLayer == -1) lowObstacleLayer = 7;
        midObstacleLayer = LayerMask.NameToLayer("MidObstacle");
        highObstacleLayer = LayerMask.NameToLayer("HighObstacle");
        itemHealLayer = LayerMask.NameToLayer("ItemHeal");
        itemPowerUpLayer = LayerMask.NameToLayer("ItemPowerUp");
        groundLayer = LayerMask.NameToLayer("Ground");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        Time.timeScale = 1f;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        ChangeState(runningState);
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    //Sistema de Deteccion de Colision con suelo
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == groundLayer)
        {
            if (currentState == jumpingState)
            {
                jumpingState.OnGrounded();
            }
        }
        /*
        if(other.gameObject.layer == groundLayer)
        {
            isGrounded = true;
            isJumping = false;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
         if(other.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }*/
    }

    //Detector de obstaculos e items
    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerObject = other.gameObject.layer;

        /*bool isRunning = isGrounded && !isSliding && !isJumping;*/

        //Bomba
        if (layerObject == lowObstacleLayer && currentState != jumpingState)
        {
            TakeDamagePercent(lowDamagePercent);
            Destroy(other.gameObject);
        }

        //Proyectiles
        else if (layerObject == midObstacleLayer && currentState == runningState)
        {
            TakeDamagePercent(midDamagePercent);
            Destroy(other.gameObject);
        }
        //Kinsecto
        else if (layerObject == highObstacleLayer && currentState == jumpingState)
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
        if (currentState == deadState || isInvincible) return;

        int damageAmount = Mathf.RoundToInt(maxHealth * (percentage/100f));
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            ChangeState(deadState);
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    //Sistema Curas y que no exceda max vida
    public void HealHealth(int amount)
    {
        if (currentState == deadState) return;

        currentHealth += amount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
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
            spriteRenderer.material.SetFloat("_Invincible", 1);
            //spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 0.8f);
        }

        yield return new WaitForSeconds(duration);

        isInvincible=false;

        if(spriteRenderer != null)
        {
            spriteRenderer.material.SetFloat("_Invincible", 0);
            //spriteRenderer.color = Color.white;
        }

    }


    //Sistema de Inputs
    public void OnJump(InputValue value)
    {
        if (currentState == deadState) return;
        currentState.HandleJumpInput();
    }

    public void OnCrouch(InputValue value)
    {
        if (currentState == deadState) return;
        currentState.HandleCrouchInput(value.isPressed);
    }
    
    public void OnRestartAndPause(InputValue value)

    {
        if(!value.isPressed) return;

        if(currentState == deadState)
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

        OnPauseChanged?.Invoke(isPaused);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Level 0");
    }
}
