using Zenject;

public class TavernInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<OrdersPanelUI>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}
