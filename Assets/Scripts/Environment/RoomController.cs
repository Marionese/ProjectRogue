using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class RoomController : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<EnemyScript> enemyPrefabs;
    [SerializeField] List<Door> doors;
    public event System.Action<EnemyScript> OnSpawnEnemy;
    private List<EnemyScript> activeEnemies = new List<EnemyScript>();

    void Awake()
    {
        GameSession.Instance.RegisterRoom(this);
    }
    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        foreach (var point in spawnPoints)
        {
            var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            EnemyScript spawned = Instantiate(enemy, point.position, Quaternion.identity);

            activeEnemies.Add(spawned);
            spawned.OnDeath += HandleEnemyDeath;
            OnSpawnEnemy.Invoke(spawned);
        }
    }

    void HandleEnemyDeath(EnemyScript e)
    {
        activeEnemies.Remove(e);

        if (activeEnemies.Count == 0)
        {
            OpenDoors();
        }
    }
    void OpenDoors()
    {
        foreach (var door in doors)
        {
            door?.OpenDoor();
        }
    }
}


