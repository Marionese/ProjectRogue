using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GameObject gameSessionPrefab;

    void Awake()
    {
        if (GameSession.Instance == null)
            Instantiate(gameSessionPrefab);
    }
}
