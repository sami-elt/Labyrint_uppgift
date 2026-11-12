
using System.Collections.Generic;

public static class BFS
{
    public static List<int> FindPath(
        List<Edge>[] graph,
        int start,
        int goal)
    {
        Queue<int> q = new Queue<int>();
        q.Enqueue(start);

        Dictionary<int, int> from =
            new Dictionary<int, int>();

        from[start] = start;

        bool goalFound = false;

        while (q.Count > 0)
        {
            int currentId = q.Dequeue();

            if (currentId == goal)
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

        if (!goalFound)
        {
            return null;
        }

        List<int> path = new List<int>();
        int curID = goal;

        while (curID != start)
        {
            path.Add(curID);
            curID = from[curID];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }
}
