using UnityEngine;
using System.Collections.Generic;

public class GameSession : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool twoPlayer;
    public static GameSession Instance;
    public List<ItemData> runItemPool = new();
    public List<ItemData> player1RunItems = new();
    public List<ItemData> player2RunItems = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); return;
        }
        Instance = this; DontDestroyOnLoad(gameObject);
        BuildRunPool();
    }

    // Functions
    public void JoinCoop()
    {
        twoPlayer = true;
    }
    public void LeaveCoop()
    {
        twoPlayer = false;
    }
    public void RegisterRunItem(int playerIndex, ItemData item)
    {
        if (playerIndex == 0) player1RunItems.Add(item);
        if (playerIndex == 1) player2RunItems.Add(item);
    }
    public bool AnyPlayerHasItem(string itemID)
    {
        // Check bei Spieler 1
        if (player1RunItems.Exists(i => i.itemID == itemID))
            return true;

        // Check bei Spieler 2
        if (player2RunItems.Exists(i => i.itemID == itemID))
            return true;

        // Niemand besitzt es
        return false;
    }
    public ItemData PickItemFromRunPool()
    {
        if (runItemPool.Count == 0)
            return null;

        int index = Random.Range(0, runItemPool.Count);
        ItemData chosen = runItemPool[index];

        // Item aus dem Pool entfernen = einzigartig im Run
        runItemPool.RemoveAt(index);

        return chosen;
    }
    private void BuildRunPool()
    {
        // liest ALLE ItemData Assets im Projekt
        ItemData[] allItems = Resources.LoadAll<ItemData>("Items");

        runItemPool = new List<ItemData>(allItems);

        Debug.Log("Run pool created with " + runItemPool.Count + " items.");
    }
}
