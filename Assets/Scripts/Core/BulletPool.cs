using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialSize = 30;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Pre-warm the pool
        for (int i = 0; i < initialSize; i++)
        {
            GameObject b = Instantiate(bulletPrefab);
            b.SetActive(false);
            pool.Enqueue(b);
        }
    }

    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            var b = pool.Dequeue();
            b.SetActive(true);
            return b;
        }
        else
        {
            // Expand pool if needed
            GameObject b = Instantiate(bulletPrefab);
            return b;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
