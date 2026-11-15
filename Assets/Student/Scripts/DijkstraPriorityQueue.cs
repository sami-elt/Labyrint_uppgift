using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Student.Scripts
{
    public class DijkstraPriorityQueue
    {
        public static List<int> FindPath(List<Edge>[] graph, int start, int goal)
        {
            int n = graph.Length;
            float[] distance = new float[n];
            int[] previous = new int[n];
            bool[] visited = new bool[n];

            for (int i = 0; i < n; i++)
            {
                distance[i] = float.PositiveInfinity;
                previous[i] = -1;

            }
            distance[start] = 0f;

            MinHeap queue = new MinHeap();
            queue.Push(start, 0f);

            while (queue.Count > 0)
            {
                var result = queue.Pop();
                int current = result.node;

                if (visited[current])
                {
                    continue;
                }
                visited[current] = true;

                if (current  == goal)
                {
                    break;
                }

                foreach (var edge in graph[current])
                {
                    int neighbor = edge.To;

                    float newDistance = distance[current] + edge.Cost;

                    if (!visited[neighbor] && newDistance < distance[neighbor])
                    {
                        distance[neighbor] = newDistance;

                        previous[neighbor] = current;

                        queue.Push(neighbor, newDistance);
                    }
                }
              
            }

            if (distance[goal] == float.PositiveInfinity)
            {
                return new List<int>();
            }


            List<int> path = new List<int>();

            int node = goal;

            while (node != -1)
            {
                path.Add(node);
                node = previous[node];

            }
            path.Reverse();



            return path;
        }
    }
}
