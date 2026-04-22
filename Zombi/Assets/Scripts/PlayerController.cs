using UnityEngine;

public class PlayerController : Character
{
    private float invincibilityTime = 1.5f;
    private float invincibilityTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        rb.linearDamping = 8f;
        health = 5;
    }

    private void Start()
    {
        spriteRenderer.color = new Color(0.2f, 0.6f, 1f);
        transform.localScale = Vector3.one * 1.2f;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 direction = (mousePos - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, mousePos);

        if (distance > 0.5f)
        {
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            float alpha = Mathf.PingPong(Time.time * 15f, 1f);
            spriteRenderer.color = new Color(0.2f, 0.6f, 1f, 0.3f + alpha * 0.7f);
        }
        else
        {
            spriteRenderer.color = new Color(0.2f, 0.6f, 1f);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (invincibilityTimer > 0) return;

        base.TakeDamage(damage);
        invincibilityTimer = invincibilityTime;

        GameObject zombie = GameObject.FindGameObjectWithTag("Zombie");
        if (zombie != null)
        {
            Vector2 knockback = (transform.position - zombie.transform.position).normalized;
            rb.AddForce(knockback * 3f, ForceMode2D.Impulse);
        }

        if (health <= 0)
        {
            GameManager.Instance.GameOver();
            Die();
        }
    }

    
    public void SetMaxHealth(int newHealth)
    {
        health = newHealth;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}