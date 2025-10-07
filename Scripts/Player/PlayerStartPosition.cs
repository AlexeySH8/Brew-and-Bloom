using UnityEngine;
using Zenject;

public class PlayerStartPosition : MonoBehaviour
{
    private PlayerController _playerController;

    [Inject]
    public void Construct(PlayerController playerController)
    {
        _playerController = playerController;
    }

    private void Awake()
    {
        _playerController.gameObject.transform.position = transform.position;
    }
}
