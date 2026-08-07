using UnityEngine;

public class BackgroundDecorationSpawner : MonoBehaviour
{
    [Header("Lista de Prefabs Decorativos")]
    public GameObject[] decorationPrefabs;

    [Header("Tiempos de Spawn")]
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 6f;

    [Header("Posición de Aparición")]
    public float spawnX = 12f;
    public float spawnY = -1.5f;

    private float timer;
    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        if (GameManager.instance != null && (!GameManager.instance.hasStarted || GameManager.instance.gameOver))
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnRandomDecoration();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    void SpawnRandomDecoration()
    {
        if (decorationPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, decorationPrefabs.Length);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, transform.position.z);

        Instantiate(decorationPrefabs[randomIndex], spawnPosition, Quaternion.identity);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
