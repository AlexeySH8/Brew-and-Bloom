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

        Container.Bind<ItemStandManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<Shop>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<Seller>()
            .FromComponentInNewPrefab(_seller)
            .AsSingle();

        Container.Bind<ButPen>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<ShopServiceFactory>()
            .AsSingle();

        Container.Bind<AddButService>()
            .AsTransient();

        Container.Bind<PortalUI>()
            .FromComponentInHierarchy()
            .AsSingle().NonLazy();
    }
}

