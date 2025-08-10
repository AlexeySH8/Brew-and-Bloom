using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[field: SerializeField] public float Speed { get; private set; }
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalInput, float verticalInput)
    {
        _rb.velocity = new Vector2(horizontalInput, verticalInput).normalized * _speed;
        Vector2 input = new Vector2(horizontalInput, verticalInput).normalized;
        Vector2 move = input * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + move);
    }
}
