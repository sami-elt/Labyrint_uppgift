
using System.Collections.Generic;
using UnityEngine;

public static class BFS
{
    public static List<int> FindPath(
        MazeGraph graph,
        int start,
        int goal)
    {
        Queue<int> queue = new Queue<int>();

        HashSet<int> visited = new HashSet<int>();

        Dictionary<int, int> previous =
            new Dictionary<int, int>();

        queue.Enqueue(start);
        visited.Add(start);


        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Edge edge in graph.GetNeighbors(current))
            {
                int neighbor = edge.To;

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    previous[neighbor] = current;
                }
            }

        }



        if (!visited.Contains(goal))
        {
            return null;
        }

        List<int> path = new List<int>();

        int node = goal;

        while (node != start)
        {
            path.Add(node);
            node = previous[node];
        }

        path.Add(start);

        path.Reverse();

        return path;

    }

    public static void TestBFS()
    {
        MazeGraph graph = new MazeGraph(3);

        graph.AddEdge(0, 1, 1f);
        graph.AddEdge(1, 2, 1f);
        graph.AddEdge(0, 2, 5f);

        List<int> path = FindPath(graph, 0, 2);

        if (path == null)
        {
            Debug.LogError("BFS hittade ingen väg.");
            return;
        }

        Debug.Log("BFS väg: " + string.Join(" -> ", path));
    }

}
