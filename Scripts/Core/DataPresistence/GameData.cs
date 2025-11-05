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
    public List<GuestSaveData> GuestsSaveData = new();
    public List<SceneItemData> ScenesItemsData = new();
    public List<ItemHolderSaveData> ItemHoldersSaveData = new();
    public List<SoilSaveData> SoilsSaveData = new();
    public List<int> CompletedDishes = new();
}

[Serializable]
public class GuestSaveData
{
    public string GuestId;
    public int DialoguePartIndex;
    public bool IsServed;
    public CurrentOrderSaveData CurrentOrderSaveData;
}

[Serializable]
public class CurrentOrderSaveData
{
    public int Payment;
    public int IngredientsMask;
    public bool IsCompleted;
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
public class ItemHolderSaveData
{
    public string HolderId;
    public string PrefabPath;
}

[Serializable]
public class SoilSaveData
{
    public string SoilId;
    public int CultivationStage;
    public SeedSaveData SeedSaveData;
}

[Serializable]
public class SeedSaveData
{
    public string SeedPrefabPath;
    public int growthStageIndex;
}
