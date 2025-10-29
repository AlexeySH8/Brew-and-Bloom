using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int Balance;
    public int DailyEarning;
    public string SavedSceneName;
    public bool IsToolSpawned;
    public int PowderSpawnersCount;
    public int CurrentPowderCount;
    public List<SceneItemData> ScenesItems = new();
    public List<ItemHolderData> ItemHolders = new();
    public List<SoilData> SoilsData = new();
}

[Serializable]
public class SceneItemData
{
    public string SceneName;
    public List<ItemSaveData> ItemsSaveData = new();
}

[Serializable]
public class ItemSaveData
{
    public string PrefabPath;
    public float[] Position = new float[3];
    public float[] Rotation = new float[4];
}

[Serializable]
public class ItemHolderData
{
    public string HolderId;
    public string PrefabPath;
}

[Serializable]
public class SoilData
{
    public string SoilId;
    public int CultivationStage;
}
