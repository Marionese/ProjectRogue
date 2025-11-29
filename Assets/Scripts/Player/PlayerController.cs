using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerBaseStats baseStats; // Drag im Inspector
    public PlayerRuntimeStats runtimeStats { get; private set; }

    [SerializeField] private int playerID;
    public int PlayerID => playerID;

    [Header("Modules")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInteraction interaction;

    void Awake()
    {
        // Optional: falls du mal vergisst zuzuweisen
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (interaction == null) interaction = GetComponent<PlayerInteraction>();

    }

    void Start()
    {
        // Runtime-Stats initialisieren
        if (runtimeStats == null)
            runtimeStats = new PlayerRuntimeStats(baseStats);

        // Movement bekommt Zugriff auf Stats
        if (movement != null)
            movement.Initialize(runtimeStats);

        // Kamera registrieren
        CameraTarget camTarget = FindFirstObjectByType<CameraTarget>();
        if (camTarget != null)
        {
            camTarget.Register(transform);
        }
    }

    void OnDestroy()
    {
        if (CameraTarget.Instance != null)
            CameraTarget.Instance.Unregister(transform);
    }

    // ---------- INPUT-FORWARDING ----------

    public void OnMove(InputValue val)
    {
        if (movement == null) return;
        Vector2 input = val.Get<Vector2>();
        movement.SetMoveInput(input);
    }

    void OnJump(InputValue val)
    {
        if (movement == null) return;
        movement.SetJump(val.isPressed);
    }

    void OnDrop()
    {
        if (movement == null) return;
        movement.DropThroughPlatform();
    }

    void OnInterract(InputValue val)
    {
        if (interaction == null) return;
        interaction.Interact(gameObject);
    }

    // ---------- API / Hilfsmethoden ----------

    public void SetPlayerID(int id)
    {
        playerID = id;
    }

    public void ResetRuntimeStats()
    {
        runtimeStats = new PlayerRuntimeStats(baseStats);

        if (movement != null)
            movement.Initialize(runtimeStats);

        // HP ggf. auch neu setzen
        // if (health != null)
        //     health.SetMaxHP(runtimeStats.maxHP, refill: true);
    }

    // Wrapper, damit bestehender Code weiter funktioniert:
    public void DamagePlayer(int dmg)
    {
        runtimeStats.currentHP -= dmg;
        if (runtimeStats.currentHP <= 0)
            HandleDeath();
    }
    public void HealPlayer(int healAmount)
    {
        if (runtimeStats.currentHP < runtimeStats.maxHP)
            runtimeStats.currentHP = Mathf.Min(runtimeStats.currentHP + healAmount, runtimeStats.maxHP);
    }

    void HandleDeath()
    {
        GameSession.Instance.OnPlayerDied(this);
    }
}
