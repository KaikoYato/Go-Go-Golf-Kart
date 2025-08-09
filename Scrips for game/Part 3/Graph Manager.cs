using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager instance;
    public Graphimplementation<Transform> graphWaypoints = new Graphimplementation<Transform>();
    [SerializeField]private List<Transform> SortedWaypoints = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildGraphStuff();
    }

    private void buildGraphStuff()
    { 
        GraphWaypoints[] collectedWaypoints = FindObjectsOfType<GraphWaypoints>();

        // Sort by name to get it in order like a1 b1 c1
        SortedWaypoints = collectedWaypoints
            .OrderBy(w =>
            {
               
                string name = w.name; 
                string letterPart = new string(name.TakeWhile(char.IsLetter).ToArray()); // gets the string part
                string numberPart = new string(name.SkipWhile(char.IsLetter).ToArray()); // Gets the number part
                int number = int.TryParse(numberPart, out var n) ? n : 0; // makes the number part into an int
                return (letterPart, number); // returns and sorts it
            })
            .Select(w => w.transform)
            .ToList();

        foreach (Transform waypoint in SortedWaypoints)
        {
            graphWaypoints.AddVertex(waypoint);
        }

        foreach (GraphWaypoints point in collectedWaypoints)
        {
            Transform from = point.transform;

            foreach (Transform connectedPoint in point.ConnectedWaypoint)// adding the vertex, each waypoint has an array of waypoints it would be connected to  like A1 has b1 in its array. Idk it just makes sense in my head
            {
                graphWaypoints.AddVertex(connectedPoint);// adding the vertex just incase for some odd reason it didnt add it in the beginning , it will add it here, uh the add vertex method for the graph should deal with the fact that if there is a duplicate, it shouldnt add it
                graphWaypoints.AddEdge(from, connectedPoint); // adds an edge from the og point to the connected point, so for instance, A1 would be from B1 would be the connected point
            }
        }
    }
    
    public Transform GetFirstWayPoint()
    {
        return SortedWaypoints.Count > 0 ? SortedWaypoints[0] : null; // gets the first waypoint, usually sets the target for the ai, used for purely tracking shit on the player
    }

    public List<Transform> GetCurrentConnectedWaypoints(Transform from)
    {
        return graphWaypoints.GetConnectedVertices(from); // helper method, used for ai branching, this literally gets the potential connect vertices & edges from the current wayPoint, B1 should return c1 and c2
    }
    
    public bool IsConnectedTo(Transform from, Transform to)
    {
        // this method returns what the current waypoint is connected to, this is mostly for the player stuff, to check if its crossing a correct waypoint
        List<Transform> connected = GetCurrentConnectedWaypoints(from);
        if (connected == null) return false;
        return connected.Contains(to);
    }
}
