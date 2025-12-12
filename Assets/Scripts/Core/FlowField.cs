using System.Collections.Generic;
using UnityEngine;

public class FlowField : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;
    public Vector2 origin = Vector2.zero;

    public LayerMask obstacleMask;

    public FlowNode[,] grid;

    private Vector2Int lastPlayerCell = new Vector2Int(-999, -999);

    void Awake()
    {
        origin = transform.position;
        GenerateGrid();
    }

    // --------------------------------------------------
    // GRID GENERATION (baseWalkable is permanent)
    // --------------------------------------------------
    public void GenerateGrid()
    {
        grid = new FlowNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = GridToWorld(x, y);

                bool walkable = !Physics2D.OverlapBox(
                    worldPos,
                    Vector2.one * (cellSize * 0.9f),
                    0,
                    obstacleMask
                );

                grid[x, y] = new FlowNode()
                {
                    worldPos = worldPos,
                    cost = float.MaxValue,
                    flowDirection = Vector2.zero,
                    baseWalkable = walkable,
                    walkable = walkable // dynamic layer starts same as base
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

        // Spielerzelle walkable
        MakeRingWalkable(cell, 1); // Radius 1 = 3x3 Feld

        if (cell == lastPlayerCell)
            return;

        lastPlayerCell = cell;

        ComputeDistanceField(cell);
        ComputeFlowDirections();
    }

    void MakeRingWalkable(Vector2Int center, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                Vector2Int p = new Vector2Int(center.x + dx, center.y + dy);

                if (!InBounds(p))
                    continue;

                grid[p.x, p.y].walkable = true;
            }
        }
    }


    // Reset walkable layer to baseWalkable
    private void ResetWalkableToBase()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].walkable = grid[x, y].baseWalkable;
            }
        }
    }

    // --------------------------------------------------
    // BFS COST FIELD
    // --------------------------------------------------
    void ComputeDistanceField(Vector2Int start)
    {
        foreach (var node in grid)
            node.cost = float.MaxValue;

        Queue<Vector2Int> q = new Queue<Vector2Int>();

        grid[start.x, start.y].cost = 0;
        q.Enqueue(start);

        Vector2Int[] dirs =
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,1),
            new Vector2Int(-1,-1)
        };

        while (q.Count > 0)
        {
            Vector2Int c = q.Dequeue();
            float currentCost = grid[c.x, c.y].cost;

            foreach (var d in dirs)
            {
                Vector2Int n = c + d;

                if (!InBounds(n))
                    continue;

                FlowNode node = grid[n.x, n.y];
                if (!node.walkable)
                    continue;

                float movementCost = (d.x != 0 && d.y != 0) ? 1.4f : 1f;

                if (node.cost > currentCost + movementCost)
                {
                    node.cost = currentCost + movementCost;
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
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,1),
            new Vector2Int(-1,-1)
        };

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                FlowNode node = grid[x, y];

                float bestCost = node.cost;
                Vector2 bestDir = Vector2.zero;

                foreach (var d in dirs)
                {
                    Vector2Int n = new Vector2Int(x, y) + d;

                    if (!InBounds(n))
                        continue;

                    FlowNode neigh = grid[n.x, n.y];

                    if (neigh.cost < bestCost)
                    {
                        bestCost = neigh.cost;
                        bestDir = (neigh.worldPos - node.worldPos).normalized;
                    }
                }

                node.flowDirection = bestDir;
            }
        }
    }

    // --------------------------------------------------
    // UTILITY
    // --------------------------------------------------
    public Vector2 GetFlowDirection(Vector3 worldPos)
    {
        Vector2Int g = WorldToGrid(worldPos);

        if (!InBounds(g))
            return Vector2.zero;

        return grid[g.x, g.y].flowDirection;
    }

    public Vector2Int WorldToGrid(Vector3 pos)
    {
        int x = Mathf.FloorToInt((pos.x - origin.x) / cellSize);
        int y = Mathf.FloorToInt((pos.y - origin.y) / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(
            origin.x + x * cellSize + cellSize * 0.5f,
            origin.y + y * cellSize + cellSize * 0.5f
        );
    }

    bool InBounds(Vector2Int g)
    {
        return g.x >= 0 && g.x < width && g.y >= 0 && g.y < height;
    }

    void OnDrawGizmos()
    {
        if (grid == null)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                FlowNode n = grid[x, y];

                Gizmos.color = n.walkable ? Color.white : Color.red;

                Vector3 pos = GridToWorld(x, y);
                Gizmos.DrawWireCube(pos, Vector3.one * cellSize);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(pos, pos + (Vector3)n.flowDirection * (cellSize * 0.4f));
            }
        }
    }
}

public class FlowNode
{
    public Vector2 worldPos;
    public float cost;
    public Vector2 flowDirection;

    public bool baseWalkable; // From grid generation
    public bool walkable;     // Dynamic state
}
