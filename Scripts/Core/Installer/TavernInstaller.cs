using UnityEngine;
using Zenject;

public class TavernInstaller : MonoInstaller
{
    [SerializeField] private PlayerController _playerControllerPref;

    public override void InstallBindings()
    {
        Container.Bind<PlayerController>()
            .FromComponentInNewPrefab(_playerControllerPref)
            .AsSingle();

        Container.Bind<OrdersPanelUI>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}
