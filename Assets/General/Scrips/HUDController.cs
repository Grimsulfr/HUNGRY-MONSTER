using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    //Referencias UI
    public Slider healthSlider;
    public TextMeshProUGUI scoreText;

    //Referencias Juego
    private PlayerMovement player;
    private float currentScore = 0;
    public float scoreSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();

        if (player != null && healthSlider != null)
        {
            healthSlider.maxValue = player.maxHealth;
            healthSlider.value = player.currentHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && healthSlider != null)
        {
            healthSlider.value = player.currentHealth;
        }

        if (player != null && scoreSpeed > 0)
        {
            currentScore += scoreSpeed * Time.deltaTime;

            if(scoreText != null)
            {
                scoreText.text = $"Distance: {Mathf.FloorToInt(currentScore)} m";
            }
        }
    }
}
