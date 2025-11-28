using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameSession : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool twoPlayer;
    public static GameSession Instance;
    public List<ItemData> normalPool = new();
    public List<ItemData> shopPool = new();
    public List<ItemData> bossPool = new();
    public List<ItemData> player1RunItems = new();
    public List<ItemData> player2RunItems = new();
    public PlayerRuntimeStats.Snapshot player1Snapshot;
    public PlayerRuntimeStats.Snapshot player2Snapshot;
    
    //MetaProgression + SaveSlot
    public int metaCoins;
    public int currentSlot = 1; // default
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
    public void RegisterRunItem(int playerID, ItemData item)
    {
        if (playerID == 0)
            player1RunItems.Add(item);
        else
            player2RunItems.Add(item);
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
    public ItemData PickFromPool(List<ItemData> pool)
    {
        if (pool.Count == 0)
            return null;

        int index = Random.Range(0, pool.Count);
        ItemData chosen = pool[index];
        if (chosen.isUnique)
        {
            pool.RemoveAt(index);
        }
        return chosen;
    }
    private void BuildRunPool()
    {
        // liest ALLE ItemData Assets im Projekt
        ItemData[] allItems = Resources.LoadAll<ItemData>("Items");

        foreach (var item in allItems)
        {
            switch (item.category)
            {
                case ItemCategory.Normal:
                    normalPool.Add(item);
                    break;

                case ItemCategory.Shop:
                    shopPool.Add(item);
                    break;

                case ItemCategory.Boss:
                    bossPool.Add(item);
                    break;
            }
        }
    }
    public void SavePlayerSnapshot(int playerID, PlayerRuntimeStats.Snapshot snapshot)
    {
        if (playerID == 0) player1Snapshot = snapshot;
        else player2Snapshot = snapshot;
    }
    public void SaveAllSnapshots()
    {
        var players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (var p in players)
        {
            var snap = p.runtimeStats.CreateSnapshot();
            SavePlayerSnapshot(p.PlayerID, snap);
        }
    }
    public PlayerRuntimeStats.Snapshot GetPlayerSnapshot(int playerID)
    {
        return (playerID == 0) ? player1Snapshot : player2Snapshot;
    }
    public void ResetSession()
    {
        player1RunItems.Clear();
        player2RunItems.Clear();

        player1Snapshot = default;
        player2Snapshot = default;

        twoPlayer = false;

        normalPool.Clear();
        shopPool.Clear();
        bossPool.Clear();
        BuildRunPool();

        Debug.Log("GameSession reset!");
    }
    public void OnPlayerDied(PlayerController deadPlayer)
    {
        // Wenn Coop NICHT aktiv: sofort Game Over
        if (!twoPlayer)
        {
            StartCoroutine(HandleGameOver());
            return;
        }

        // Coop aktiv → checken ob zweiter Spieler noch lebt
        bool otherAlive = false;

        var players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (var p in players)
        {
            if (p != deadPlayer && p.runtimeStats.currentHP > 0)
                otherAlive = true;
        }

        if (otherAlive)
        {
            // Einer tot – Coop geht weiter
            // später: Resurrection Item, Revive System, etc.
            Destroy(deadPlayer.gameObject);
            return;
        }

        // Beide tot → Game Over
        StartCoroutine(HandleGameOver());
    }
    private IEnumerator HandleGameOver()
    {
        Debug.Log("GAME OVER");

        ResetSession();

        yield return new WaitForSeconds(0.2f); // verhindert Race-Conditions

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }


}
