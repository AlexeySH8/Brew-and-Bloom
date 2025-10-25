using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataPath;
    private string _fileName;

    public FileDataHandler(string dataPath, string fileName)
    {
        _dataPath = dataPath;
        _fileName = fileName;
    }

    public GameData LoadData()
    {
        string fullPath = Path.Combine(_dataPath, _fileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"LOADING ERROR with Path: {fullPath} : {e}");
            }
        }
        return loadedData;
    }

    public void SaveData(GameData gameData)
    {
        string fullPath = Path.Combine(_dataPath, _fileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SAVING ERROR with Path: {fullPath} : {e}");
        }
    }
}
