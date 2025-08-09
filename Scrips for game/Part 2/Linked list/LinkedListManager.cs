using System;
using System.Linq;
using UnityEngine;

public class LinkedListManager : MonoBehaviour
{
    public static LinkedListManager instance;

    public  OurLinkedList<Transform> waypoints = new OurLinkedList<Transform>();

    public GameObject[] waypointsInWorld;
    
    private void Awake()
    { // making it an instance so  it doesnt kak out
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        waypointsInWorld = GameObject.FindGameObjectsWithTag("Waypoint");
        waypointsInWorld = waypointsInWorld.OrderBy(w => 
        {
            string name = w.name;
            string numberStr = name.Substring(name.LastIndexOf('_') + 1); 
            int number = int.Parse(numberStr); 
            return number; 
        }).ToArray();// getting all the waypoints and ordering it by the last number in the name

        for (int i = 0; i < waypointsInWorld.Length; i++)
        {
            waypoints.Insert(waypointsInWorld[i].transform); // adds this to our linked list
        }
       
        
    }
    
    
    public  Transform GetWaypoint(int index)
    {
        if (index >= 0 && index < waypoints.Size)
            return waypoints[index]; // funny thing, ill explain to nj. ITS A HELPER METHOD, SO I CAN RETURN THE TRANSFORM OF THE WAYPOINT DEPENDING ON where the ai index is tracking
        return null;// if there is no index it will return null because there isnt any waypoint in that linked list at that index
    }

    public  int ListSize => waypoints.Size; // size bs
}
