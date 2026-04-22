using UnityEngine;

public class ZombieController : Character
{
    private Transform player;
    private float stunTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        rb.linearDamping = 5f;
        health = 1;
    }

    private void Start()
    {
        spriteRenderer.color = new Color(0.2f, 0.9f, 0.2f);
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Vector2 away = (transform.position - collision.transform.position).normalized;
            rb.AddForce(away * 1f, ForceMode2D.Impulse);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 away = (transform.position - collision.transform.position).normalized;
            rb.AddForce(away * 2f, ForceMode2D.Impulse);
            stunTimer = 0.3f;

            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null) pc.TakeDamage(1);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}