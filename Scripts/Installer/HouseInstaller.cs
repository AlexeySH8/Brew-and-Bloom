using UnityEngine;
using Zenject;

public class HouseInstaller : MonoInstaller
{
    [SerializeField] Seller _seller;

    public override void InstallBindings()
    {
        Container.Bind<Shop>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<Seller>()
            .FromComponentInNewPrefab(_seller)
            .AsSingle();
    }
}

