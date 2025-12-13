using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class RoomController : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<EnemyBase> enemyPrefabs;
    [SerializeField] List<Door> doors;
    public event System.Action<EnemyBase> OnSpawnEnemy;
    private List<EnemyBase> activeEnemies = new List<EnemyBase>();

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

            EnemyBase spawned = Instantiate(enemy, point.position, Quaternion.identity);

            activeEnemies.Add(spawned);
            spawned.OnDeath += HandleEnemyDeath;
            OnSpawnEnemy.Invoke(spawned);
        }
    }

    void HandleEnemyDeath(EnemyBase e)
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


