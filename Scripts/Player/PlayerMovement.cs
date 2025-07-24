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
        _rb.velocity = new Vector2(horizontalInput, verticalInput).normalized * _speed;
    }
}
