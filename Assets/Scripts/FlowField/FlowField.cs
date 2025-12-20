using System.Collections.Generic;
using UnityEngine;

public class FlowField : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;
    public float fixedY = 0f;

    public Vector3 origin = Vector3.zero;
    public LayerMask obstacleMask;

    public FlowNode[,] grid;

    private Vector2Int lastPlayerCell = new Vector2Int(-999, -999);

    void Awake()
    {
        origin = transform.position;
        fixedY = origin.y;
        GenerateGrid();
    }

    // --------------------------------------------------
    // GRID GENERATION
    // --------------------------------------------------
    public void GenerateGrid()
    {
        grid = new FlowNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 worldPos = GridToWorld(x, z);

                bool walkable = Physics.OverlapBox(
                    worldPos,
                    new Vector3(cellSize * 0.45f, 0.5f, cellSize * 0.45f),
                    Quaternion.identity,
                    obstacleMask
                ).Length == 0;


                grid[x, z] = new FlowNode
                {
                    worldPos = worldPos,
                    cost = float.MaxValue,
                    flowDirection = Vector3.zero,
                    baseWalkable = walkable,
                    walkable = walkable
                };
            }
        }
    }

    // --------------------------------------------------
    // FLOWFIELD UPDATE
    // --------------------------------------------------
    public void UpdateFlowField(Vector3 playerPos)
    {
        Vector2Int cell = WorldToGrid(playerPos);

        if (!InBounds(cell))
            return;

        ResetWalkableToBase();


        if (cell == lastPlayerCell)
            return;

        lastPlayerCell = cell;

        ComputeDistanceField(cell);
        ComputeFlowDirections();
    }
    void ResetWalkableToBase()
    {
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
                grid[x, z].walkable = grid[x, z].baseWalkable;
    }

    // --------------------------------------------------
    // BFS COST FIELD
    // --------------------------------------------------
    void ComputeDistanceField(Vector2Int start)
    {
        foreach (var node in grid)
            node.cost = float.MaxValue;

        Queue<Vector2Int> q = new();
        grid[start.x, start.y].cost = 0;
        q.Enqueue(start);

        Vector2Int[] dirs =
        {
            new(1,0), new(-1,0), new(0,1), new(0,-1),
            new(1,1), new(1,-1), new(-1,1), new(-1,-1)
        };

        while (q.Count > 0)
        {
            Vector2Int c = q.Dequeue();
            float currentCost = grid[c.x, c.y].cost;

            foreach (var d in dirs)
            {
                Vector2Int n = c + d;
                if (!InBounds(n)) continue;

                FlowNode node = grid[n.x, n.y];
                if (!node.walkable) continue;

                float moveCost = (d.x != 0 && d.y != 0) ? 1.4f : 1f;

                if (node.cost > currentCost + moveCost)
                {
                    node.cost = currentCost + moveCost;
                    q.Enqueue(n);
                }
            }
        }
    }

    // --------------------------------------------------
    // FLOW DIRECTIONS
    // --------------------------------------------------
    void ComputeFlowDirections()
    {
        Vector2Int[] dirs =
        {
            new(1,0), new(-1,0), new(0,1), new(0,-1),
            new(1,1), new(1,-1), new(-1,1), new(-1,-1)
        };

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                FlowNode node = grid[x, z];

                float bestCost = node.cost;
                Vector3 bestDir = Vector3.zero;

                foreach (var d in dirs)
                {
                    Vector2Int n = new(x + d.x, z + d.y);
                    if (!InBounds(n)) continue;

                    FlowNode neigh = grid[n.x, n.y];
                    if (neigh.cost < bestCost)
                    {
                        bestCost = neigh.cost;
                        bestDir = (neigh.worldPos - node.worldPos);
                        bestDir.y = 0f;
                        bestDir.Normalize();
                    }
                }

                node.flowDirection = bestDir;
            }
        }
    }

    // --------------------------------------------------
    // UTILITY
    // --------------------------------------------------
    public Vector3 GetFlowDirection(Vector3 worldPos)
    {
        Vector2Int g = WorldToGrid(worldPos);
        if (!InBounds(g)) return Vector3.zero;
        return grid[g.x, g.y].flowDirection;
    }

    public Vector2Int WorldToGrid(Vector3 pos)
    {
        int x = Mathf.FloorToInt((pos.x - origin.x) / cellSize);
        int z = Mathf.FloorToInt((pos.z - origin.z) / cellSize);
        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(int x, int z)
    {
        return new Vector3(
            origin.x + x * cellSize + cellSize * 0.5f,
            fixedY,
            origin.z + z * cellSize + cellSize * 0.5f
        );
    }

    bool InBounds(Vector2Int g)
    {
        return g.x >= 0 && g.x < width && g.y >= 0 && g.y < height;
    }

    void OnDrawGizmos()
    {
        if (grid == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                FlowNode n = grid[x, z];
                Vector3 pos = GridToWorld(x, z);

                Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, 0.1f, cellSize));

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(pos, pos + n.flowDirection * (cellSize * 0.4f));
            }
        }
    }
}


public class FlowNode
{
    public Vector3 worldPos;
    public float cost;
    public Vector3 flowDirection;

    public bool baseWalkable;
    public bool walkable;
}

