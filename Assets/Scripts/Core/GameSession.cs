using UnityEngine;

public class GameSession : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool twoPlayer;
    public static GameSession Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); return;
        }
        Instance = this; DontDestroyOnLoad(gameObject);
    }

    // Functions
    public void JoinCoop()
    {
        twoPlayer = true;
    }
    public void LeaveCoop()
    {
        twoPlayer = false;
    }
}
