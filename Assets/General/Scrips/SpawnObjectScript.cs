using UnityEngine;

public class SpawnObjectScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float baseSpeed = 3f;
    private float lifeTime = 6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifeTime);
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float speedMult = GameManager.instance.speedMultiplier;
        rb.linearVelocity = Vector2.left * (baseSpeed * speedMult);
    }
}
