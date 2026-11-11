
using System.Collections.Generic;

using UnityEngine;

public class MazeGraph
{
    private List<Edge>[] adjacency;

    public MazeGraph(int numberOfNodes)
    {
        adjacency = new List<Edge>[numberOfNodes];

        for (int i = 0; i < numberOfNodes; i++)
        {
            adjacency[i] = new List<Edge>();
        }
    }

    public void AddEdge(int from, int to, float cost)
    {
        adjacency[from].Add(new Edge(to, cost));
    }

    public List<Edge> GetNeighbors(int node)
    {
        return adjacency[node];
    }

    public static MazeGraph BuildGraph(IMapData mapData)
    {
        int width = mapData.Width;
        int height = mapData.Height;

        MazeGraph graph = new MazeGraph(width * height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int currentNode = y * width + x;
               

                // höger & vänster
                if (x + 1 < width)
                {
                    float cost = mapData.GetVerticalWallCost(x + 1, y);

                    if (!float.IsPositiveInfinity(cost) &&
                        cost < float.MaxValue)
                    {
                        int rightNode = y * width + (x + 1);

                        float movementCost = Mathf.Max(1f, cost);

                        graph.AddEdge(currentNode, rightNode, movementCost);
                        graph.AddEdge(rightNode, currentNode, movementCost);
                    }
                }

                // upp & ner
                if (y + 1 < height)
                {
                    float cost = mapData.GetHorizontalWallCost(x, y + 1);

                    if (!float.IsPositiveInfinity(cost) &&
                        cost < float.MaxValue)
                    {
                        int upperNode = (y + 1) * width + x;

                        float movementCost = Mathf.Max(1f, cost);

                        graph.AddEdge(currentNode, upperNode, movementCost);
                        graph.AddEdge(upperNode, currentNode, movementCost);
                    }
                }


                Vector2Int currentPos = new Vector2Int(x, y);

                foreach (JumpConnection jump in mapData.GetJumpsFrom(currentPos))
                {
                    Vector2Int targetPos = jump.To;

                    if (targetPos.x >= 0 && targetPos.x < width &&
                        targetPos.y >= 0 && targetPos.y < height)
                    {
                        int targetNode = targetPos.y * width + targetPos.x;

                        graph.AddEdge(currentNode, targetNode, jump.Cost);
                    }
                }



            }
        }

        return graph;
    }

    public static void TestGraph()
    {
        MazeGraph graph = new MazeGraph(4);

        graph.AddEdge(0, 1, 1f);
        graph.AddEdge(0, 2, 5f);

        foreach (Edge edge in graph.GetNeighbors(0))
        {
            Debug.Log("Från 0 till " + edge.To +
                      " kostnad: " + edge.Cost);
        }
    }


}
