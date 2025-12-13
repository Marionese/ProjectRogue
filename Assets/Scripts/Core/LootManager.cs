using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    List<EnemyBase> enemies = new List<EnemyBase>();
    [SerializeField] RoomController roomController;
    [SerializeField] CoinPickup coin;
    RoomController currentRoom;
   void Awake()
    {
        currentRoom = FindFirstObjectByType<RoomController>();
        currentRoom.OnSpawnEnemy += HandleEnemySpawned;
    }
    void HandleEnemySpawned(EnemyBase spawned)
    {
        enemies.Add(spawned);
        Debug.Log("Enemy List right now:" +enemies);
        spawned.OnDeath += HandleEnemyDeath;

    }
    void HandleEnemyDeath(EnemyBase enemy)
    {
        DropLoot(enemy);
        enemies.Remove(enemy);
    }
    void DropLoot(EnemyBase enemy)
    {
        Instantiate(coin,enemy.transform.position, Quaternion.identity);
    }
}
