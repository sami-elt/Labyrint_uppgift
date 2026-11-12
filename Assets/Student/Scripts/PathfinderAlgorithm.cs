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


    public static List<Vector2Int> FindShortestPath(
        Vector2Int start,
        Vector2Int goal,
        IMapData mapData)
    {
        width = mapData.Width;
        height = mapData.Height;

        // Samis graf
        GraphBuilder(mapData);

        int startId = ToId(start);
        int goalId = ToId(goal);

        //  Dijkstra
        //List<int> result = Dijkstra.FindPath(
        //    graph,
        //    startId,
        //    goalId
        //);

        // BFS
        // För att testa BFS 
        List<int> result = BFS.FindPath(graph, startId, goalId);
        Debug.Log("BFS : Activ");

        if (result == null)
        {
            Debug.LogWarning("Ingen väg hittades.");
            return null;
        }

        List<Vector2Int> path = new List<Vector2Int>();

        foreach (int node in result)
        {
            path.Add(ToPos(node));
        }


        // Kontrollera kostnaden
        float totalCost = 0f;

        for (int i = 0; i < result.Count - 1; i++)
        {
            int fromId = result[i];
            int toId = result[i + 1];


            float cheapestCost = float.PositiveInfinity;

            foreach (Edge edge in graph[fromId])
            {
                if (edge.To == toId && edge.Cost < cheapestCost)
                {
                    cheapestCost = edge.Cost;
                }
            }

            if (float.IsPositiveInfinity(cheapestCost))
            {
                Debug.LogError("Kant saknas i grafen!");
                return null;
            }

            totalCost += cheapestCost;

        }

        Debug.Log("Dijkstra beräknad kostnad: " + totalCost);

        Debug.Log("Dijkstra hittade en väg!");
        Debug.Log("Antal noder: " + path.Count);

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

                        if (!IsMovementBlocked(currentPosition, nPosition, mapData))
                        {
                            int nId = ToId(nPosition);

                            float wallCost;

                            if (d.x != 0)
                            {
                                // Höger eller vänster
                                int wallX = d.x > 0 ? nPosition.x : currentPosition.x;

                                wallCost = mapData.GetVerticalWallCost(
                                    wallX,
                                    currentPosition.y
                                );
                            }
                            else
                            {
                                // Upp eller ner
                                int wallY = d.y > 0 ? nPosition.y : currentPosition.y;

                                wallCost = mapData.GetHorizontalWallCost(
                                    currentPosition.x,
                                    wallY
                                );
                            }

                            // En vanlig förflyttning kostar minst 1
                            float cost = Mathf.Max(1f, wallCost);

                            graph[currentId].Add(new Edge(nId, cost));
                        }

                    }
                }

                // Lägg till riktade jumps från aktuell ruta
                foreach (JumpConnection jump in mapData.GetJumpsFrom(currentPosition))
                {
                    Vector2Int target = jump.To;

                    // Kontrollera att målet finns i kartan
                    if (target.x < 0 || target.x >= width ||
                        target.y < 0 || target.y >= height)
                    {
                        continue;
                    }

                    // Dijkstra kräver icke-negativa kostnader
                    if (float.IsNaN(jump.Cost) ||
                        float.IsInfinity(jump.Cost) ||
                        jump.Cost < 0f)
                    {
                        continue;
                    }

                    int targetId = ToId(target);

                    graph[currentId].Add(
                        new Edge(targetId, jump.Cost)
                    );
                }


            }

        }


    }

    public static bool IsMovementBlocked(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        //if (mapData.TryGetJumpCost(from, to, out _))
        //    return false;

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
