using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected float speed = 5f;
    protected int health = 5;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    public int Health => health;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}