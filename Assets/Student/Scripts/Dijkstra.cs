
using System.Collections.Generic;



// Dijkstras algoritm utan prioritetskö.
// Hittar vägen med lägst total kostnad mellan start och mål.
// Tidskomplexitet: O(V² + E), där V = noder och E = bågar.

public static class Dijkstra
{
    public static List<int> FindPath(
        List<Edge>[] graph,
        int start,
        int goal)
    {
        // Antalet noder i grafen.
        int nodeCount = graph.Length;

        // distance: lägsta kända kostnad till varje nod.
        // previous: föregående nod för att återskapa vägen.
        // visited: markerar färdigbehandlade noder.
        float[] distance = new float[nodeCount];
        int[] previous = new int[nodeCount];
        bool[] visited = new bool[nodeCount];

        // Sätt alla avstånd till oändligheten.
        // -1 betyder att noden ännu saknar föregångare.
        for (int i = 0; i < nodeCount; i++)
        {
            distance[i] = float.PositiveInfinity;
            previous[i] = -1;
        }


        // Kostnaden från startnoden till sig själv är 0.
        distance[start] = 0f;


        // Behandla högst V noder.
        for (int i = 0; i < nodeCount; i++)
        {
            int current = -1;
            float minDistance = float.PositiveInfinity;

            // Sök linjärt efter den obesökta nod
            // som har lägst känd kostnad.
            // Denna sökning tar O(V) tid per varv.
            for (int j = 0; j < nodeCount; j++)
            {
                if (!visited[j] && distance[j] < minDistance)
                {
                    minDistance = distance[j];
                    current = j;
                }
            }
            // Avsluta om inga fler noder kan nås.
            if (current == -1)
            {
                break;
            }
            // Markera noden som färdigbehandlad
            visited[current] = true;

            // Avsluta när målet har nåtts.
            if (current == goal)
            {
                break;
            }

            // Undersök alla utgående bågar från noden.
            foreach (Edge edge in graph[current])
            {
                int neighbor = edge.To;

                // Hoppa över redan färdigbehandlade noder.
                if (visited[neighbor])
                {
                    continue;
                }

                // Beräkna kostnaden via den aktuella noden.
                float newDistance = distance[current] + edge.Cost;

                // Uppdatera om en billigare väg hittas.
                if (newDistance < distance[neighbor])
                {
                    distance[neighbor] = newDistance;
                    previous[neighbor] = current;
                }
            }

        }


        // Returnera null om målet inte kan nås.
        if (float.IsPositiveInfinity(distance[goal]))
        {
            return null;
        }

        // Återskapa vägen genom att följa previous bakåt.
        List<int> path = new List<int>();

        int node = goal;

        while (node != start)
        {
            
            path.Add(node);
            node = previous[node];
        }
        // Lägg till startnoden.
        path.Add(start);

        // Vänd listan så att vägen går från start till mål.
        path.Reverse();

        return path;

    }
}
