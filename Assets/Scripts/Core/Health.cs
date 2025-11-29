using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHP = 3f;
    [SerializeField] private float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    public event Action OnDied;
    public event Action<float, float> OnHealthChanged; // current, max

    void Awake()
    {
        if (currentHP <= 0)
            currentHP = maxHP;
    }

    public void SetMaxHP(float value, bool refill = true)
    {
        maxHP = value;
        if (refill)
            currentHP = maxHP;

        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        currentHP -= amount;
        if (currentHP < 0f) currentHP = 0f;

        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP == 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;

        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    void Die()
    {
        OnDied?.Invoke();
    }
}
