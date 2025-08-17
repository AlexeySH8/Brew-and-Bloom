using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalInput, float verticalInput)
    {
        Vector2 input = new Vector2(horizontalInput, verticalInput).normalized;
        Vector2 move = input * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + move);
    }
}
