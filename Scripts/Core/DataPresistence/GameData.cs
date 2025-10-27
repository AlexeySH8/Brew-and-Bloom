using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int Balance;
    public int DailyEarning;
    public List<ItemSaveData> ItemsSaveData = new();
}

[Serializable]
public class ItemSaveData
{
    public string PrefabPath;
    public float[] Position = new float[3];
    public float[] Rotation = new float[4];
}
