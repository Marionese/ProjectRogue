using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetPath(int slot)
    {
        return Application.persistentDataPath + "/save_slot" + slot + ".json";
    }

    public static void SaveGame(int slot)
    {
        SaveData data = new SaveData
        {
            metaCoins = GameSession.Instance.metaCoins
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Saved slot " + slot);
    }

    public static void LoadGame(int slot)
    {
        string path = GetPath(slot);

        if (!File.Exists(path))
        {
            Debug.Log("No save file found for slot " + slot);
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        GameSession.Instance.metaCoins = data.metaCoins;

        Debug.Log("Loaded slot " + slot);
    }

    public static bool SlotExists(int slot)
    {
        return File.Exists(GetPath(slot));
    }
}
