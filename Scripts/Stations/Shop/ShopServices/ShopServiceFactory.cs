using UnityEngine;
using Zenject;

public class ShopServiceFactory
{
    private readonly DiContainer _container;

    public ShopServiceFactory(DiContainer container)
    {
        _container = container;
    }

    public IShopService Create(ShopServiceType type)
    {
        return type switch
        {
            ShopServiceType.AddBut => _container.Instantiate<AddButService>(),
            ShopServiceType.EndGame => _container.Instantiate<EndGameService>(),
            _ => null
        };
    }
}
