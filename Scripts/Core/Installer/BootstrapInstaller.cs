using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private Recipes _recipesPref;
    [SerializeField] private DataPersistenceManager _dataPersistenceManagerPref;
    [SerializeField] private GuestSaveSystem _guestSaveSystemPref;
    [SerializeField] private GameSceneManager _gameSceneManagerPref;
    [SerializeField] private ItemPool _itemPoolPref;
    [SerializeField] private SFX _sfxPref;
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
        Container.Bind<IDataPersistenceManager>()
            .FromComponentInNewPrefab(_dataPersistenceManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<GameSceneManager>()
            .FromComponentInNewPrefab(_gameSceneManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<MusicManager>()
            .FromComponentInNewPrefab(_musicManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<SFX>()
            .FromComponentInNewPrefab(_sfxPref)
            .AsSingle().NonLazy();

        Container.Bind<GuestsManager>()
            .FromComponentInNewPrefab(_guestsManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<GuestSaveSystem>()
           .FromComponentInNewPrefab(_guestSaveSystemPref)
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
