using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private Recipes _recipesPref;
    [SerializeField] private PlayerData _playerDataPref;
    [SerializeField] private GameManager _gameManagerPref;
    [SerializeField] private SFXManager _sfxManagerPref;
    [SerializeField] private MusicManager _musicManagerPref;
    [SerializeField] private DayManager _dayManagerPref;
    [SerializeField] private GuestsManager _guestsManagerPref;
    [SerializeField] private OrdersManager _ordersManagerPref;
    [SerializeField] private PlayerController _playerControllerPref;

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

        Container.Bind<MusicManager>()
            .FromComponentInNewPrefab(_musicManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<SFXManager>()
            .FromComponentInNewPrefab(_sfxManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<DayManager>()
            .FromComponentInNewPrefab(_dayManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<GuestsManager>()
            .FromComponentInNewPrefab(_guestsManagerPref)
            .AsSingle().NonLazy();

        Container.Bind<OrdersManager>()
            .FromComponentInNewPrefab(_ordersManagerPref)
            .AsSingle().NonLazy();
    }

    private void BindPlayer()
    {
        Container.Bind<IPlayerInput>()
            .To<PCInput>().AsSingle();

        Container.Bind<PlayerData>()
            .FromInstance(_playerDataPref).AsSingle();

        Container.Bind<PlayerWallet>().AsSingle();

        Container.Bind<PlayerController>()
            .FromComponentInNewPrefab(_playerControllerPref)
            .AsSingle();
    }

    private void BindRecipes()
    {
        Container.Bind<Recipes>()
            .FromInstance(_recipesPref).AsSingle();

        _recipesPref.Initialize();
    }
}
