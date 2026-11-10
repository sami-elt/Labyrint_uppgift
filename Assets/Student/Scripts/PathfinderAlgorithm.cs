using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public int To;
    public float Cost;

    public Edge(int to, float cost) { 
    
        To = to;
        Cost = cost;
    }
}

public static class PathfindingAlgorithm
{
    // TODO: Implement the pathfinding assignment here.
    // Find the lowest-cost path from start to goal using the map dimensions, wall costs and directed jumps exposed by IMapData.

    private static int width;
    private static int height;

    private static int ToId(Vector2Int pos)
    {
        return pos.y * width + pos.x;
    }

    private static Vector2Int ToPos(int id)
    {
        return new Vector2Int(id % width, id / width);
    }

    public static List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        // Most of your solution should be implemented in this method.

        width = mapData.Width;
        height = mapData.Height;
        Debug.Log("id " + ToId(start));
        Debug.Log("ruta: " + ToPos(ToId(start)));

        Debug.LogWarning("FindShortestPath is not implemented.");
        return null;
    }

    public static bool IsMovementBlocked(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        if (mapData.TryGetJumpCost(from, to, out _))
            return false;

        int deltaX = to.x - from.x;
        int deltaY = to.y - from.y;

        if (Mathf.Abs(deltaX) + Mathf.Abs(deltaY) != 1)
            return true;

        if (deltaX != 0)
        {
            int wallX = deltaX > 0 ? to.x : from.x;
            float wallCost = mapData.GetVerticalWallCost(wallX, from.y);
            return float.IsPositiveInfinity(wallCost) || wallCost >= float.MaxValue;
        }

        int wallY = deltaY > 0 ? to.y : from.y;
        float horizontalWallCost = mapData.GetHorizontalWallCost(from.x, wallY);
        return float.IsPositiveInfinity(horizontalWallCost) || horizontalWallCost >= float.MaxValue;
    }
}
