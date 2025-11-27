using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    public GameObject playerPrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private GameObject player1;
    private GameObject player2;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Spawn Player 1 ALWAYS
        SpawnPlayer1();

        // Spawn Player 2 AUTOMATICALLY if coop is active
        if (GameSession.Instance.twoPlayer)
        {
            SpawnPlayer2();
        }
    }

    public GameObject SpawnPlayer1()
    {
        if (player1 != null) return player1;

        player1 = Instantiate(playerPrefab, spawnPoint1.position, Quaternion.identity);
        player1.name = "Player1";
        var controller = player1.GetComponent<PlayerController>();
        controller.SetPlayerID(0);

        var pm = player1.GetComponent<PlayerModifierManager>();
        pm.InitializeFromSession(0);

        return player1;
    }

    public GameObject SpawnPlayer2()
    {
        if (player2 != null) return player2;

        player2 = Instantiate(playerPrefab, spawnPoint2.position, Quaternion.identity);
        player2.name = "Player2";
        var mov = player2.GetComponent<PlayerController>();
        mov.SetPlayerID(1);

        var pm = player2.GetComponent<PlayerModifierManager>();
        pm.InitializeFromSession(1);

        return player2;
    }

}

