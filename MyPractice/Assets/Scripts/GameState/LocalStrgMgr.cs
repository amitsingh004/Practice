using System;
using System.IO;
using UnityEngine;

public class LocalStrgMgr
{
    private const string SaveFileName = "MyGameSaveData.json";
    private const string SaveFolder = "GameSaves";
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFolder, SaveFileName);

    public void SaveGame(GameSaveState gameSaveState)
    {
        try
        {
            string directoryPath = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string jsonData = JsonUtility.ToJson(gameSaveState, true);
            File.WriteAllText(SaveFilePath, jsonData);
            Debug.Log($"Game saved to: {SaveFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    public void LoadGame(out GameSaveState gameSaveState)
    {
        gameSaveState = null;
        try
        {
            if (File.Exists(SaveFilePath))
            {
                string jsonData = File.ReadAllText(SaveFilePath);
                gameSaveState = JsonUtility.FromJson<GameSaveState>(jsonData);
                Debug.Log($"Game loaded from: {SaveFilePath}");
            }
            else
            {
                Debug.LogWarning("No save file found.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }
    public bool HasSavedGame()
    {
        return File.Exists(SaveFilePath);
    }
     public void DeleteSaveData()
    {
        if (HasSavedGame())
        {
            File.Delete(SaveFilePath);
            Debug.Log($"Save file deleted: {SaveFilePath}");
        }
    }
}
