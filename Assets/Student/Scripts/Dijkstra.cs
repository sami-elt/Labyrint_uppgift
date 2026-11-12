
using System.Collections.Generic;

public static class Dijkstra
{
    public static List<int> FindPath(
        List<Edge>[] graph,
        int start,
        int goal)
    {
        int nodeCount = graph.Length;

        float[] distance = new float[nodeCount];
        int[] previous = new int[nodeCount];
        bool[] visited = new bool[nodeCount];

        for (int i = 0; i < nodeCount; i++)
        {
            distance[i] = float.PositiveInfinity;
            previous[i] = -1;
        }

        distance[start] = 0f;


        for (int i = 0; i < nodeCount; i++)
        {
            int current = -1;
            float minDistance = float.PositiveInfinity;

            for (int j = 0; j < nodeCount; j++)
            {
                if (!visited[j] && distance[j] < minDistance)
                {
                    minDistance = distance[j];
                    current = j;
                }
            }

            if (current == -1)
            {
                break;
            }

            visited[current] = true;


            if (current == goal)
            {
                break;
            }

            foreach (Edge edge in graph[current])
            {
                int neighbor = edge.To;

                if (visited[neighbor])
                {
                    continue;
                }

                float newDistance = distance[current] + edge.Cost;

                if (newDistance < distance[neighbor])
                {
                    distance[neighbor] = newDistance;
                    previous[neighbor] = current;
                }
            }

        }



        if (float.IsPositiveInfinity(distance[goal]))
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
}
