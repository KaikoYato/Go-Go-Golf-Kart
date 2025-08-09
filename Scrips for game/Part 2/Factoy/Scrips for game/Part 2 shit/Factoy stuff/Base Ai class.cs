using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public abstract class BaseAiclass : MonoBehaviour
{
   // its soo much cleaner now YAY
   public NavMeshAgent agent;
    public int currentWaypoint = 0;
    public int totalWaypoints;
    public int leaderboardWaypointIndex = 0;
    public Transform currentTargetGraphWaypoint{ get; private set; }
        //public int level = 2;
        private bool Level3;
    public abstract void ChangeStats();

    private void Awake()
    {
        Level3 = SceneManager.GetActiveScene().name.Contains("3");
    }

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
      
           if (Level3)
         {
           
             currentTargetGraphWaypoint = GraphManager.instance.GetFirstWayPoint();

           
         }
        //SetNextGraphDestination();
    }
   public void SetNextDestination()
    {
        if (!Level3)
        {
            Transform targetDestination = LinkedListManager.instance.GetWaypoint(currentWaypoint); // helper method comes in clutch, it will send the index in which the ai needs to go to currently
            if (targetDestination != null)
            {
                agent.SetDestination(targetDestination.position); // if that method returns something it will set the destination
            }
        }
    }

    public void SetNextDestinaionGraph()
    {
        if (Level3)
        {
            if (currentTargetGraphWaypoint != null)
            {
                agent.SetDestination(currentTargetGraphWaypoint.position); // setting the next destination
            }
        }
    }

    public void GetNextGraphWaypoint()
    {
        // this is for the randomization bullshit, there is probably a better way to do this but it will randomly select an index of the connected waypoints, so if
        // a waypoint has just one , it will obv pick that one but if it has two it will randomly pick between the two
        List<Transform> nextWaypoints = GraphManager.instance.GetCurrentConnectedWaypoints(currentTargetGraphWaypoint);
        if (nextWaypoints != null && nextWaypoints.Count > 0)
        {
            int rand = Random.Range(0, nextWaypoints.Count);
            Transform selectedGraphWaypoint = nextWaypoints[rand];
            
//            Debug.Log($"{gameObject.name} at {currentGraphWaypoint.name} branching to {selectedGraphWaypoint.name}");

            
            currentTargetGraphWaypoint = selectedGraphWaypoint;
        }
    }
   private  void IncrementWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % LinkedListManager.instance.ListSize;// oh this one im proud of, because the ai needs to wrap back to the first waypoint, what this is doing is once it currentwaypoint reaches the max of the linked list size it will return 0 because im using mod
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Level3)
        {
            if (other.CompareTag("Waypoint") &&
                other.transform ==
                LinkedListManager.instance
                    .GetWaypoint(currentWaypoint)) // if it is crossing the waypoint, cant cheat yk
            {
                totalWaypoints++;
                Debug.Log("Entered trigger with object: " + gameObject.name);
                Debug.Log("Number of way Points : " + totalWaypoints);
                IncrementWaypoint();
                SetNextDestination();
            }
        }

        if (Level3)
        {// level 3 trigger bs
            if (other.CompareTag("Waypoint") && other.transform == currentTargetGraphWaypoint)
            {
                //   Debug.Log("this is triggering");
                totalWaypoints++;
                GetNextGraphWaypoint();
                SetNextDestinaionGraph();
                if (leaderboardWaypointIndex < IgnoreBranchesManager.instance.idealGraphPath.Count && other.transform ==
                    IgnoreBranchesManager.instance.idealGraphPath[leaderboardWaypointIndex])
                {
                    Debug.Log("TEYUDGFTHSJDGFH test");
                    leaderboardWaypointIndex = (leaderboardWaypointIndex + 1) % IgnoreBranchesManager.instance.idealGraphPath.Count;
                }
            }

           
        }
        
    }

}
