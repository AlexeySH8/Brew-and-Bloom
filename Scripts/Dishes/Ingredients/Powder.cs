public class Powder : Ingredient
{
    private PowderSpawner _powderSpawner;

    public void Init(PowderSpawner powderSpawner)
    {
        _powderSpawner = powderSpawner;
    }

    public override void Discard()
    {
        _powderSpawner?.OnPowderDestroy();
        base.Discard();
    }
}
