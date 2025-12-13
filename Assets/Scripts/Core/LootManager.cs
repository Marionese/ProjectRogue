using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    List<EnemyScript> enemies = new List<EnemyScript>();
    [SerializeField] RoomController roomController;
    [SerializeField] CoinPickup coin;
    RoomController currentRoom;
   void Awake()
    {
        currentRoom = FindFirstObjectByType<RoomController>();
        currentRoom.OnSpawnEnemy += HandleEnemySpawned;
    }
    void HandleEnemySpawned(EnemyScript spawned)
    {
        enemies.Add(spawned);
        Debug.Log("Enemy List right now:" +enemies);
        spawned.OnDeath += HandleEnemyDeath;

    }
    void HandleEnemyDeath(EnemyScript enemy)
    {
        DropLoot(enemy);
        enemies.Remove(enemy);
    }
    void DropLoot(EnemyScript enemy)
    {
        Instantiate(coin,enemy.transform.position, Quaternion.identity);
    }
}
