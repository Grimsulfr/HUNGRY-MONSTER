using UnityEngine;

public class LoopingGround : MonoBehaviour
{
    [Header("Velocidad")]
    public float baseSpeed = 10f;

    [Header("Configuración de Loop")]
    [Tooltip("Pon aquí la cantidad de piezas de suelo idénticas que colocaste (por ejemplo: 3)")]
    public int totalCopies = 3;

    private float spriteWidth;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            spriteWidth = sr.bounds.size.x;
        }
        else
        {
            Debug.LogError($"[LoopingGround] Falta el SpriteRenderer en {gameObject.name}");
        }
    }

    void Update()
    {
        if (GameManager.instance != null && (!GameManager.instance.hasStarted || GameManager.instance.gameOver))
        {
            return;
        }

        float gameSpeed = (GameManager.instance != null) ? GameManager.instance.speedMultiplier : 1f;
        float currentSpeed = baseSpeed * gameSpeed;

        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        float spriteRightEdge = transform.position.x + (spriteWidth / 2f);

        if (spriteRightEdge < cameraLeftEdge)
        {
            transform.position += new Vector3(spriteWidth * totalCopies, 0f, 0f);
        }
    }
}
