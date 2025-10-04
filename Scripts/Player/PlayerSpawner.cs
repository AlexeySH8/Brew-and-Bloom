using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPref;
    [SerializeField] private float _spawnForce;
    [SerializeField] private Vector2 _forceVector;

    private void Awake()
    {
        GameObject exictingPlayer = GameObject.FindWithTag("Player");
        if (exictingPlayer == null)
        {
            Instantiate(_playerPref, transform.position, transform.rotation);
        }
        else
        {
            exictingPlayer.transform.position = transform.position;
        }
    }

    private void OnValidate()
    {
        _forceVector.x = Mathf.Clamp01(_forceVector.x);
        _forceVector.y = Mathf.Clamp01(_forceVector.y);
    }
}
