using UnityEngine;

public class MovingDecoration : MonoBehaviour
{
    public float parallaxSpeedMultiplier = 0.5f;

    public float baseSpeed = 10f;
    public float destroyX = -15f;

    void Update()
    {
        if (GameManager.instance != null && (!GameManager.instance.hasStarted || GameManager.instance.gameOver))
        {
            return;
        }

        float gameSpeed = (GameManager.instance != null) ? GameManager.instance.speedMultiplier : 1f;
        float currentSpeed = baseSpeed * gameSpeed * parallaxSpeedMultiplier;

        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}