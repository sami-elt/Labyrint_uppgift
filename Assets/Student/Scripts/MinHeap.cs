
using System;
using System.Collections.Generic;

// Egen implementation av en binär MinHeap.
// Används som prioritetskö i Dijkstras algoritm.
// Noden med lägst kostnad ligger alltid först.
public class MinHeap
{
    // Lagrar nodens ID och dess kostnad.
    // Heapens ordning baseras på kostnaden.
    private List<(int node, float cost)> heap =
        new List<(int node, float cost)>();

    // Returnerar antalet element i heapen.
    // Tidskomplexitet: O(1).
    public int Count => heap.Count;

    // Lägger till en nod med tillhörande kostnad.
    // Tidskomplexitet: O(log H).
    public void Push(int node, float cost)
    {
        // Lägg det nya elementet sist i listan.
        heap.Add((node, cost));

        int i = heap.Count - 1;

        // Flytta elementet uppåt tills heapens
        // ordning är korrekt (heapify up).
        while (i > 0)
        {
            // Beräkna förälderns index.
            int parent = (i - 1) / 2;

            // Avsluta om föräldern har lägre
            // eller samma kostnad.
            if (heap[i].cost >= heap[parent].cost)
            {
                break;
            }

            // Byt plats med föräldern.
            var temp = heap[i];
            heap[i] = heap[parent];
            heap[parent] = temp;

            // Fortsätt jämföra från förälderns position.
            i = parent;
        }
    }

    // Hämtar och tar bort noden med lägst kostnad.
    // Tidskomplexitet: O(log H).
    public (int node, float cost) Pop()
    {
        // Kontrollera att heapen inte är tom.
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("Heap är tom");
        }

        // Roten innehåller alltid lägsta kostnaden.
        var min = heap[0];

        // Flytta sista elementet till roten
        // och ta bort den sista positionen.
        int lastIndex = heap.Count - 1;

        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int i = 0;

        // Flytta elementet nedåt tills heapens
        // ordning är återställd (heapify down).
        while (true)
        {
            // Beräkna index för vänster och höger barn.
            int left = (2 * i) + 1;
            int right = (2 * i) + 2;

            int smallest = i;

            // Kontrollera om vänster barn har lägre kostnad.
            if (left < heap.Count &&
                heap[left].cost < heap[smallest].cost)
            {
                smallest = left;
            }

            // Kontrollera om höger barn har lägre kostnad.
            if (right < heap.Count &&
                heap[right].cost < heap[smallest].cost)
            {
                smallest = right;
            }

            // Avsluta om elementet redan ligger rätt.
            if (smallest == i)
            {
                break;
            }

            // Byt plats med barnet som har lägst kostnad.
            var temp = heap[i];
            heap[i] = heap[smallest];
            heap[smallest] = temp;

            // Fortsätt från barnets position.
            i = smallest;
        }

        // Returnera elementet med lägst kostnad.
        return min;
    }
}
