using UnityEngine;

using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public Transform[] groundTiles;

    public float baseSpeed = 10f;

    public float overlapFix = 0.01f;

    private float spriteWidth;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (groundTiles != null && groundTiles.Length > 0)
        {
            SpriteRenderer sr = groundTiles[0].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spriteWidth = sr.bounds.size.x;
            }
            else
            {
                Debug.LogError("[GroundManager] Los objetos de suelo necesitan tener un SpriteRenderer.");
            }
        }
    }

    void Update()
    {
        if (GameManager.instance != null && (!GameManager.instance.hasStarted || GameManager.instance.gameOver))
        {
            return;
        }

        float gameSpeed = (GameManager.instance != null) ? GameManager.instance.speedMultiplier : 1f;
        float moveAmount = baseSpeed * gameSpeed * Time.deltaTime;

        foreach (Transform tile in groundTiles)
        {
            tile.Translate(Vector3.left * moveAmount);
        }

        float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        foreach (Transform tile in groundTiles)
        {
            float rightEdge = tile.position.x + (spriteWidth / 2f);

            if (rightEdge < cameraLeftEdge)
            {
                float maxRightX = GetRightmostTileX();

                float newX = maxRightX + spriteWidth - overlapFix;
                tile.position = new Vector3(newX, tile.position.y, tile.position.z);
            }
        }
    }
    private float GetRightmostTileX()
    {
        float maxX = float.MinValue;
        foreach (Transform tile in groundTiles)
        {
            if (tile.position.x > maxX)
            {
                maxX = tile.position.x;
            }
        }
        return maxX;
    }
}