using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    //Referencias UI
    public Slider healthSlider;
    public TextMeshProUGUI scoreText;

    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    //Referencias Juego
    public PlayerMovement player;

    //suscribirse a la señal si el jugador existe
    void OnEnable()
    {
        //Player Events
        if (player != null)
        {
            player.OnHealthChanged += UpdateHealthBar;
            player.OnPauseChanged += TogglePauseUI;
        }

        //GameManager Event
        if (GameManager.instance != null)
        {
            GameManager.instance.OnDistanceChanged += UpdateDistanceUI;
            GameManager.instance.OnGameOver += ShowGameOverUI;
            GameManager.instance.OnGameStarted += OnGameStartedUI;
        }
    }

    void Start()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.OnDistanceChanged -= UpdateDistanceUI;
            GameManager.instance.OnDistanceChanged += UpdateDistanceUI;

            GameManager.instance.OnGameOver -= ShowGameOverUI;
            GameManager.instance.OnGameOver += ShowGameOverUI;

            GameManager.instance.OnGameStarted -= OnGameStartedUI;
            GameManager.instance.OnGameStarted += OnGameStartedUI;
        }
        
        if (startPanel != null) startPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    //Desuscribirse de la señal si se desactiva/destroy object
    void OnDisable()
    {   
        //Player Desubscription
        if (player != null)
        {
            player.OnHealthChanged -= UpdateHealthBar;
            player.OnPauseChanged -= TogglePauseUI;
        }

        //GM Desubscripcion
        if (GameManager.instance != null)
        {
            GameManager.instance.OnDistanceChanged -= UpdateDistanceUI;
            GameManager.instance.OnGameOver -= ShowGameOverUI;
            GameManager.instance.OnGameStarted -= OnGameStartedUI;
        }
    }

    //Llama si el Player emite señal
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        print("Se actualizo Health");
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void UpdateDistanceUI (float currentDistance)
    {
        if(scoreText != null)
        {
            scoreText.text = $"Distance: {Mathf.FloorToInt(currentDistance)} m";
        }
    }

    private void TogglePauseUI(bool isPaused)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }
    }

    private void ShowGameOverUI()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void OnGameStartedUI(bool started)
    {
        if (startPanel != null)
        {
            startPanel.SetActive(!started);
        }
    }

}
