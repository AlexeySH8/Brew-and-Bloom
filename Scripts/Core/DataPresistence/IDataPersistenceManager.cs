public interface IDataPersistenceManager
{
    GameData GameData { get; }
    void Register(IDataPersistence presistance);
    void NewGame();
    void SaveGame();
    void LoadGame();
}
