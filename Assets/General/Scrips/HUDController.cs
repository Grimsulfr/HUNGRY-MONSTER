using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    //Referencias UI
    public Slider healthSlider;
    public TextMeshProUGUI scoreText;
    public GameObject pausePanel;

    //Referencias Juego
    public PlayerMovement player;

    /*private float currentScore = 0;
    public float scoreSpeed = 10f;*/

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
        }
    }

    void Start()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.OnDistanceChanged -= UpdateDistanceUI;
            GameManager.instance.OnDistanceChanged += UpdateDistanceUI;
        }

        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
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

}
