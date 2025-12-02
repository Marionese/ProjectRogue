using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHealth;
    float currentHealt;
    void Start()
    {
        currentHealt = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Helper Funktions
    public void DamageEnemy(float amount)
    {
        currentHealt -= amount;
        if (currentHealt <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}