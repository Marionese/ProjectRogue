using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialSize = 30;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private Transform bulletContainer;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create a parent object to hold bullets
        bulletContainer = new GameObject("BulletContainer").transform;
        bulletContainer.SetParent(transform);

        // Prewarm bullets
        for (int i = 0; i < initialSize; i++)
        {
            GameObject b = Instantiate(bulletPrefab, bulletContainer);
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

        // Expand pool
        GameObject nb = Instantiate(bulletPrefab, bulletContainer);
        return nb;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
