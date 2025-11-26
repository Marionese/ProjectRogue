using UnityEngine;
using System.Collections.Generic;

public class GameSession : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool twoPlayer;
    public static GameSession Instance;
    public List<ItemData> player1RunItems = new();
    public List<ItemData> player2RunItems = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); return;
        }
        Instance = this; DontDestroyOnLoad(gameObject);
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
    public bool PlayerHasItem(int playerIndex, string itemID)
    {
        if (playerIndex == 0)
            return player1RunItems.Exists(i => i.itemID == itemID);

        return player2RunItems.Exists(i => i.itemID == itemID);
    }
}
