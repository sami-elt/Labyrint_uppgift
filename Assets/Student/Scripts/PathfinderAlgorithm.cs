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



    private static List<Edge>[] graph;
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

        Debug.Log($"START: {start}--- MÅL: {goal}");

        width = mapData.Width;
        height = mapData.Height;

        GraphBuilder(mapData);

        int startId = ToId(start);
        int goalId = ToId(goal);
        Debug.Log($"GRAFEN finns, start ruta: {startId} har {graph[startId].Count} möjliga vägar");



        Queue<int> q = new Queue<int>();
        q.Enqueue(startId);

        Dictionary<int, int> from = new Dictionary<int, int>();
        from[startId] = startId;

        bool goalFound = false;

        while (q.Count > 0)
        {
            int currentId = q.Dequeue();

            if (currentId == goalId)
            {
                goalFound = true;
                break;
            }


            foreach (Edge e in graph[currentId])
            {
                int nId = e.To;

                if (!from.ContainsKey(nId))
                {
                    q.Enqueue(nId);
                    from[nId] = currentId;
                }
            }
        }

        Debug.Log($"klar, vi kom fram {goalFound}");

        if (!goalFound) return null;

        List<Vector2Int> path = new List<Vector2Int>();
        int curID = goalId;

        while (curID != startId)
        {
            path.Add(ToPos(curID));

            curID = from[curID];

        }

        path.Add(ToPos(startId));
        path.Reverse();

        string pathStr = "";

        foreach (Vector2Int p in path)
        {
            pathStr = pathStr + p.ToString();
        }

        Debug.Log("steg---- " + path.Count + " väg---- " + pathStr);

        return path;

    }

    private static void GraphBuilder(IMapData mapData)
    {
        //lägger in rutor i vår array
        int nodes = width * height;
        graph = new List<Edge>[nodes];

        Vector2Int[] direction = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                Vector2Int currentPosition = new Vector2Int(x, y);

                //ge kordinaterna ID
                int currentId = ToId(currentPosition);

                //tom lista för denna rutans väg
                graph[currentId] = new List<Edge>();

                foreach(Vector2Int d in direction)
                {
                    Vector2Int nPosition = currentPosition + d;

                    
                    if(nPosition.x >= 0 && nPosition.x < width &&  nPosition.y >= 0 && nPosition.y < height)
                    {
                        //check för vägg
                        if(!IsMovementBlocked(currentPosition,nPosition,mapData))
                        {
                            //skapa en kant dit.
                            int nId = ToId(nPosition);
                            graph[currentId].Add(new Edge(nId, 1f));


                        }
                    }
                }

            }

        }


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
