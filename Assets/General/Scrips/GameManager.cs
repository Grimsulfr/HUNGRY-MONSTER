using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Evento para Overver HUD
    public event Action<float> OnDistanceChanged;

    //Spawn Settings
    public static GameManager instance;
    public GameObject spawnObject;
    public GameObject[] spawnPoint;
    public float timer;
    public float timeBetweenSpawns;

    //Gameloop setings
    public float speedMultiplier;
    public float maxspeed;

    public bool gameOver;

    public float Distance { get; private set;}

    //Interfaz de Usuario
    public Text distanceUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameOver = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetStats();
    }

    public void ResetStats()
    {
        Distance = 0f;
        speedMultiplier = 1f;
        timer = 0f;
        gameOver = false;

        OnDistanceChanged?.Invoke(Distance);
    }

    public void SetGameOver()
    {
        gameOver = true;
        speedMultiplier = 0f;
    }

    // Update is called once per frame
    void Update()
    {   
        if (gameOver)
        {
            return;
        }

        //Limit
        if (speedMultiplier >= maxspeed)
        {
            speedMultiplier = maxspeed;
        }

        //Multiplicador de velocidad de juego
        speedMultiplier += Time.deltaTime * 0.1f;

        //Estadisticas Aumentables
        timer += Time.deltaTime;
        Distance += Time.deltaTime * 10f * speedMultiplier;

        //Emitir la señal actual
        OnDistanceChanged?.Invoke(Distance);

        //UI Contador Distancia
        if(distanceUI != null)
        {
            distanceUI.text = "Distance: " + Distance.ToString("F2");
        }

        
    }
}
