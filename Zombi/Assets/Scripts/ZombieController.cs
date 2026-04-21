using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private Transform _player;
    private float _stunTimer;

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _rb.gravityScale = 0f;
        _rb.linearDamping = 5f;
    }

    private void Start()
    {
        _sprite.color = new Color(0.2f, 0.9f, 0.2f);
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        if (_stunTimer > 0)
        {
            _stunTimer -= Time.deltaTime;
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (_player.position - transform.position).normalized;
        _rb.linearVelocity = direction * _speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Vector2 away = (transform.position - collision.transform.position).normalized;
            _rb.AddForce(away * 1f, ForceMode2D.Impulse);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 away = (transform.position - collision.transform.position).normalized;
            _rb.AddForce(away * 2f, ForceMode2D.Impulse);
            _stunTimer = 0.3f;
        }
    }
}