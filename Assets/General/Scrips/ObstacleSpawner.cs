using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Transform lowSpawnPoint;
    public Transform midSpawnPoint;
    public Transform highSpawnPoint;

    public GameObject[] lowPrefabs;

    public GameObject[] midPrefabs;

    public GameObject[] highPrefabs;

    public float timeBetweenSpawns = 3f;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > timeBetweenSpawns && !GameManager.instance.gameOver)
        {
            timer = 0f;
            SpawnRandomObject();
        }
    }
    void SpawnRandomObject()
    {
        int randomLane = Random.Range(0,3);

        switch (randomLane)
        {
            case 0:
                SpawnFromList(lowPrefabs,lowSpawnPoint);
                break;
            case 1:
                SpawnFromList(midPrefabs,midSpawnPoint);
                break;
            case 2:
                SpawnFromList(highPrefabs,highSpawnPoint);
                break;
        }
    }
    void SpawnFromList (GameObject [] prefabsList, Transform spawnPoint)
    {
        if(prefabsList == null || prefabsList.Length == 0 || spawnPoint == null)
            return;
        
        int randomIndex = Random.Range (0, prefabsList.Length);
        GameObject selectedPrefab = prefabsList[randomIndex];

        Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
    }
}
