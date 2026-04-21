using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 7f;
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private float _invincibilityTime = 1.5f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private int _currentHealth;
    private float _invincibilityTimer;

    public int Health => _currentHealth;

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void SetMaxHealth(int newHealth)
    {
        _maxHealth = newHealth;
        _currentHealth = newHealth;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _rb.gravityScale = 0f;
        _rb.linearDamping = 8f;
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        _sprite.color = new Color(0.2f, 0.6f, 1f);
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
            _rb.linearVelocity = direction * _speed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }

        if (_invincibilityTimer > 0)
        {
            _invincibilityTimer -= Time.deltaTime;
            float alpha = Mathf.PingPong(Time.time * 15f, 1f);
            _sprite.color = new Color(0.2f, 0.6f, 1f, 0.3f + alpha * 0.7f);
        }
        else
        {
            _sprite.color = new Color(0.2f, 0.6f, 1f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie") && _invincibilityTimer <= 0)
        {
            _currentHealth--;
            _invincibilityTimer = _invincibilityTime;

            Vector2 knockback = (transform.position - collision.transform.position).normalized;
            _rb.AddForce(knockback * 3f, ForceMode2D.Impulse);

            if (_currentHealth <= 0)
            {
                GameManager.Instance.GameOver();
                Destroy(gameObject);
            }
        }
    }
}