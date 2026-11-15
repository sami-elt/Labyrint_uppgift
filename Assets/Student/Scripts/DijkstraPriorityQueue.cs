
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Student.Scripts
{
    // Dijkstras algoritm med MinHeap som prioritetskö.
    // Hittar vägen med lägst total kostnad från start till mål.
    // Tidskomplexitet: O((V + E) log(V + E)).
    // V = antal noder, E = antal bågar i grafen.
    public class DijkstraPriorityQueue
    {
        public static List<int> FindPath(List<Edge>[] graph, int start, int goal)
        {
            // Antalet noder i grafen.
            int n = graph.Length;

            // distance: lägsta kända kostnad till varje nod.
            // previous: föregående nod för att återskapa vägen.
            // visited: markerar färdigbehandlade noder.
            float[] distance = new float[n];
            int[] previous = new int[n];
            bool[] visited = new bool[n];

            // Initiera alla avstånd till oändligheten.
            // -1 betyder att noden saknar föregångare.
            for (int i = 0; i < n; i++)
            {
                distance[i] = float.PositiveInfinity;
                previous[i] = -1;
            }

            // Startnodens kostnad är 0.
            distance[start] = 0f;

            // Skapa en MinHeap och lägg in startnoden.
            MinHeap queue = new MinHeap();
            queue.Push(start, 0f);

            // Fortsätt så länge det finns noder i kön.
            while (queue.Count > 0)
            {
                // Hämta noden med lägst kostnad.
                // Pop tar O(log H) tid.
                var result = queue.Pop();
                int current = result.node;

                // Samma nod kan finnas flera gånger i heapen.
                // Hoppa över den om den redan är behandlad.
                if (visited[current])
                {
                    continue;
                }

                // Markera noden som färdigbehandlad.
                visited[current] = true;

                // Avsluta om målet har nåtts.
                if (current == goal)
                {
                    break;
                }

                // Undersök alla grannar till aktuell nod.
                foreach (var edge in graph[current])
                {
                    int neighbor = edge.To;

                    // Beräkna kostnaden för vägen via current.
                    float newDistance = distance[current] + edge.Cost;

                    // Uppdatera om grannen inte är färdigbehandlad
                    // och den nya vägen har lägre kostnad.
                    if (!visited[neighbor] && newDistance < distance[neighbor])
                    {
                        distance[neighbor] = newDistance;

                        // Spara föregående nod för att bygga vägen.
                        previous[neighbor] = current;

                        // Lägg till grannen med sin nya kostnad.
                        // Push tar O(log H) tid.
                        queue.Push(neighbor, newDistance);
                    }
                }
            }

            // Om målet inte kan nås returneras en tom lista.
            if (distance[goal] == float.PositiveInfinity)
            {
                return new List<int>();
            }

            // Återskapa vägen från mål tillbaka till start.
            List<int> path = new List<int>();
            int node = goal;

            while (node != -1)
            {
                path.Add(node);
                node = previous[node];
            }

            // Vänd listan så att vägen går från start till mål.
            path.Reverse();

            return path;
        }
    }
}
