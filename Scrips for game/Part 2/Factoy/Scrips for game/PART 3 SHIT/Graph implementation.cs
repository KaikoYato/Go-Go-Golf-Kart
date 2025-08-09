using System.Collections.Generic;
using UnityEngine;

public class Graphimplementation<T>
{
    private Dictionary<T, List<T>> vertices = new Dictionary<T, List<T>>();

    public bool AddVertex(T data)
    {
        if (vertices.ContainsKey(data))
            return false;

        vertices.Add(data, new List<T>());
        return true;
    }

    public bool AddEdge(T from, T to)
    {
        if (!vertices.ContainsKey(from) || !vertices.ContainsKey(to))
            return false;

        if (vertices[from].Contains(to))
            return false;

        vertices[from].Add(to);
        return true;
    }

    public List<T> GetConnectedVertices(T vertex)
    {
        if (!vertices.ContainsKey(vertex))
            return new List<T>();

        return new List<T>(vertices[vertex]);
    }

    public void RemoveVertex(T vertex)
    {
        if (!vertices.ContainsKey(vertex))
            return;

        vertices.Remove(vertex);
        foreach (var pair in vertices.Values)
        {
            pair.Remove(vertex);
        }
    }

    public bool ContainsVertex(T vertex)
    {
        return vertices.ContainsKey(vertex);
    }

}
