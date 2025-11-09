using UnityEngine;
using Zenject;

public class HouseAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _natureSound;
    [SerializeField] private AudioSource _houseSound;

    private float _minNatureHearingX = 1f;
    private float _maxNatureHearingX = -0.5f;
    private float _minHouseHearingX = -0.5f;
    private float _maxHouseHearingX = 1f;
    private Transform _playerPos;

    [Inject]
    public void Construct(PlayerController playerController)
    {
        _playerPos = playerController.transform;
    }

    private void Update()
    {
        _natureSound.volume = Mathf.InverseLerp(
            _minNatureHearingX, _maxNatureHearingX, _playerPos.position.x);
        _houseSound.volume = Mathf.InverseLerp(
            _minHouseHearingX, _maxHouseHearingX, _playerPos.position.x);
    }
}
