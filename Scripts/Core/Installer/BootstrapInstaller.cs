using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private Recipes _recipesPref;
    [SerializeField] private GameManager _gameManagerPref;
    [SerializeField] private GameSceneManager _gameSceneManagerPref;
    [SerializeField] private ItemPool _itemPoolPref;
    [SerializeField] private SFXManager _sfxManagerPref;
    [SerializeField] private MusicManager _musicManagerPref;
    [SerializeField] private GuestsManager _guestsManagerPref;
    [SerializeField] private OrdersManager _ordersManagerPref;

    public override void InstallBindings()
    {
        BindRecipes();
        BindManagers();
        BindPlayer();
    }

    private void BindManagers()
    {
        Container.Bind<GameManager>()
            .FromComponentInNewPrefab(_gameManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<IDataPersistenceManager>()
            .To<DataPresistenceManager>()
            .AsSingle().NonLazy();

        Container.Bind<GameSceneManager>()
            .FromComponentInNewPrefab(_gameSceneManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<MusicManager>()
            .FromComponentInNewPrefab(_musicManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<SFXManager>()
            .FromComponentInNewPrefab(_sfxManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<GuestsManager>()
            .FromComponentInNewPrefab(_guestsManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<OrdersManager>()
            .FromComponentInNewPrefab(_ordersManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<ItemPool>()
            .FromComponentInNewPrefab(_itemPoolPref)
            .AsSingle().NonLazy();
    }

    private void BindPlayer()
    {
        Container.Bind<IPlayerInput>()
            .To<PCInput>().AsSingle();

        Container.Bind<PlayerWallet>().AsSingle();
    }

    private void BindRecipes()
    {
        Container.Bind<Recipes>()
            .FromInstance(_recipesPref).AsSingle();

        _recipesPref.Initialize();
    }
}
