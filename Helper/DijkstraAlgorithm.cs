using System;
using System.Collections.Generic;

public class DijkstraAlgorithm
{
    public static void Dijkstra(Dictionary<int, List<(int, int)>> graph, int source)
    {
        int n = graph.Count; 
        var distances = new Dictionary<int, int>(); 
        var visited = new HashSet<int>(); 

        foreach (var node in graph.Keys)
        {
            distances[node] = int.MaxValue;
        }
        distances[source] = 0; 

        
        var priorityQueue = new SortedDictionary<int, Queue<int>>();
        Enqueue(priorityQueue, 0, source);

        while (priorityQueue.Count > 0)
        {
            var (currentDistance, currentNode) = Dequeue(priorityQueue);

            if (visited.Contains(currentNode)) continue;
            visited.Add(currentNode);

            if (graph.TryGetValue(currentNode, out var neighbors))
            {
                foreach (var (neighbor, weight) in neighbors)
                {
                    if (visited.Contains(neighbor)) continue;
                    int newDist = currentDistance + weight;

                    if (newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        Enqueue(priorityQueue, newDist, neighbor);
                    }
                }
            }
        }

        Console.WriteLine("Shortest distances from source:");
        foreach (var node in graph.Keys)
        {
            if (distances[node] == int.MaxValue)
            {
                Console.WriteLine($"Node {node}: Unreachable");
            }
            else
            {
                Console.WriteLine($"Node {node}: {distances[node]}");
            }
        }
    }

    private static void Enqueue(SortedDictionary<int, Queue<int>> priorityQueue, int distance, int node)
    {
        if (!priorityQueue.TryGetValue(distance, out var nodes))
        {
            nodes = new Queue<int>();
            priorityQueue[distance] = nodes;
        }
        nodes.Enqueue(node);
    }

    private static (int distance, int node) Dequeue(SortedDictionary<int, Queue<int>> priorityQueue)
    {
        var firstPair = priorityQueue.First();
        var nodes = firstPair.Value;
        int node = nodes.Dequeue();
        if (nodes.Count == 0)
        {
            priorityQueue.Remove(firstPair.Key);
        }
        return (firstPair.Key, node);
    }
}
