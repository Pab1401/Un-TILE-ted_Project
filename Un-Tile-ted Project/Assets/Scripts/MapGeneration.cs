using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

public class MapGeneration : MonoBehaviour
{
    [Header("Dimensions")]
    [SerializeField] private int size = 33;
    [SerializeField] private int safeZoneSize = 3; // 3x3 area

    public struct Cells
    {
        public int block;
        public bool taken;
    }
    public Cells[,] Grid;

    public Vector2Int spawnPos;

    // Tile Constants
    public const int EMPTY = 0;
    public const int FLOOR = 1;
    public const int WALL = 2;
    public const int FOREST = 3;

    public void GenerateMap()
    {
        Grid = new Cells[size, size];

        // 1. Define Player Spawn (Bottom-Right)
        // We offset by 1 to ensure it's not literally on the index edge
        spawnPos = new Vector2Int(size - 2, size - 2);
        // Debug.Log("Player Spawn Position: " + spawnPos);

        // 2. Prepare BFS
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        // 3. Create Safe Zone (Bottom-Right)
        for (int x = spawnPos.x - 1; x <= spawnPos.x + 1; x++)
        {
            for (int y = spawnPos.y - 1; y <= spawnPos.y + 1; y++)
            {
                if (InBounds(x, y))
                {
                    Grid[x, y].block = FLOOR;
                    Vector2Int pos = new Vector2Int(x, y);
                    visited.Add(pos);
                    // Add the edges of the safe zone to the queue to start growth
                    queue.Enqueue(pos);
                }
            }
        }

        // 4. Procedural Growth (BFS)
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            Vector2Int[] neighbors = GetNeighbors(current);

            foreach (Vector2Int neighbor in neighbors)
            {
                if (InBounds(neighbor.x, neighbor.y) && !visited.Contains(neighbor))
                {
                    Grid[neighbor.x, neighbor.y].block = DetermineTileType(Grid[current.x, current.y].block, neighbor.x, neighbor.y);
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        // 5. Connectivity Check (Flood Fill)
        // Ensure everything is reachable from the Player Spawn
        EnsureConnectivity(spawnPos);
    }

    private int DetermineTileType(int parentType, int x, int y)
    {
        float roll = Random.value;
        float wallChance = 0.15f;
        float forestChance = 0.15f;

        // Clustering Logic
        if (parentType == WALL) wallChance -= 0.05f;
        if (parentType == FOREST) forestChance += 0.45f;

        // Diagonal Pinch Prevention
        if (IsDiagonalPinch(x, y)) return FLOOR;

        if (roll < forestChance) return FOREST;
        if (roll < (forestChance + wallChance)) return WALL;
        return FLOOR;
    }

    private void EnsureConnectivity(Vector2Int startPoint)
    {
        HashSet<Vector2Int> reachable = new HashSet<Vector2Int>();
        Queue<Vector2Int> q = new Queue<Vector2Int>();

        q.Enqueue(startPoint);
        reachable.Add(startPoint);

        while (q.Count > 0)
        {
            Vector2Int curr = q.Dequeue();
            foreach (var dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int next = curr + dir;
                if (InBounds(next.x, next.y) && Grid[next.x, next.y].block != WALL && !reachable.Contains(next))
                {
                    reachable.Add(next);
                    q.Enqueue(next);
                }
            }
        }

        // Pruning Pass: Convert unreachable non-walls into walls
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (Grid[x, y].block != WALL && !reachable.Contains(new Vector2Int(x, y)))
                {
                    Grid[x, y].block = WALL;
                }
            }
        }
    }

    private bool IsDiagonalPinch(int x, int y)
    {
        // Simple diagonal check against existing walls
        if (GetVal(x + 1, y + 1) == WALL && GetVal(x + 1, y) != WALL && GetVal(x, y + 1) != WALL) return true;
        if (GetVal(x - 1, y + 1) == WALL && GetVal(x - 1, y) != WALL && GetVal(x, y + 1) != WALL) return true;
        if (GetVal(x + 1, y - 1) == WALL && GetVal(x + 1, y) != WALL && GetVal(x, y - 1) != WALL) return true;
        if (GetVal(x - 1, y - 1) == WALL && GetVal(x - 1, y) != WALL && GetVal(x, y - 1) != WALL) return true;
        return false;
    }

    // Helpers
    private bool InBounds(int x, int y) => x >= 0 && x < size && y >= 0 && y < size;
    private int GetVal(int x, int y) => InBounds(x, y) ? Grid[x, y].block : EMPTY;
    private Vector2Int[] GetNeighbors(Vector2Int p) => new Vector2Int[] {
        new Vector2Int(p.x, p.y + 1), new Vector2Int(p.x, p.y - 1),
        new Vector2Int(p.x + 1, p.y), new Vector2Int(p.x - 1, p.y)
    };
}