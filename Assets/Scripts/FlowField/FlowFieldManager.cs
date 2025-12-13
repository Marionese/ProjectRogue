using UnityEngine;

public class FlowFieldManager : MonoBehaviour
{
    public static FlowFieldManager Instance;

    public FlowField flowFieldP1;
    public FlowField flowFieldP2;

    [HideInInspector] public Transform player1;
    [HideInInspector] public Transform player2;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (player1 != null)
            flowFieldP1.UpdateFlowField(player1.position);

        if (player2 != null)
            flowFieldP2.UpdateFlowField(player2.position);
    }

    public void RegisterPlayer1(Transform t)
    {
        player1 = t;
        flowFieldP1.GenerateGrid(); // optional, falls vorher Hindernisse anders waren
    }

    public void RegisterPlayer2(Transform t)
    {
        player2 = t;
        flowFieldP2.GenerateGrid();
    }
}
