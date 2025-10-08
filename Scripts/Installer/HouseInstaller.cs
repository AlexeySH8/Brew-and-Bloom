using UnityEngine;
using Zenject;

public class HouseInstaller : MonoInstaller
{
    [SerializeField] private PlayerController _playerControllerPref;
    [SerializeField] Seller _seller;

    public override void InstallBindings()
    {
        Container.Bind<PlayerController>()
            .FromComponentInNewPrefab(_playerControllerPref)
            .AsSingle();

        Container.Bind<Shop>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<Seller>()
            .FromComponentInNewPrefab(_seller)
            .AsSingle();
    }
}

