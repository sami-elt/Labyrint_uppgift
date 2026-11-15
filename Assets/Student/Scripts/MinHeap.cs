using System;
using System.Collections.Generic;

public class MinHeap
{
    // Här bygger vi vår Priority Queue
   private List<(int node, float cost)> heap = new List<(int node, float cost)>();
   public int Count => heap.Count;

    public void Push(int node, float cost)
    {
        heap.Add((node, cost));
        int i = heap.Count - 1;
       
        

        while (i > 0)
        {
            //hitta föräldrar idex
            int parent = (i - 1) / 2;

            // jämföra cost
            if (heap[i].cost >= heap[parent].cost)
            {
                break;
            }
            var temp = heap[i];
            heap[i] = heap[parent];
            heap[parent] = temp;
            
            i = parent;
        }
    }

    public (int node, float cost) Pop()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("Heap är tom");
        }

        var min = heap[0];

        int lastIndex = heap.Count - 1;

        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        int i = 0;

        while (true)
        {
            int left = (2 * i) + 1;
            int right = (2 * i) + 2;

            int smallest = i;

            // vänster
            if (left < heap.Count && heap[left].cost < heap[smallest].cost)
            {
                smallest = left;
            }
            // höger
            if (right < heap.Count && heap[right].cost < heap[smallest].cost)
            {
                smallest = right;
            }
            // 
            if (smallest == i)
            {
                break;
            }

            var temp = heap[i];
            heap[i] = heap[smallest];
            heap[smallest] = temp;

            i = smallest;

        }
        return min;

    }
}