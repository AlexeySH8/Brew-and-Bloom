public interface IDataPersistenceManager
{
    GameData GameData { get; }
    void Register(IDataPersistence presistance);
    void Unregister(IDataPersistence presistance);
    void NewGame();
    void SaveGame();
    void LoadGame();
    void LoadGameMeta();
    bool HasSave();
}
