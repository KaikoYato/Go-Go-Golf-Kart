using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostionManager : MonoBehaviour
{
     public RaceAndLapManager playerCounter;
    public TextMeshProUGUI racePositionText;

    public List<BaseAiclass> aiRacerList = new List<BaseAiclass>();
    private List<RacerInfo> previousOrder = new List<RacerInfo>();
    public string FirstPlace { get; private set; } = "Unknown";
    public List<TextMeshProUGUI> leaderboardTexts = new List<TextMeshProUGUI>();

    private bool useGraph;

    private void Start()
    {
        useGraph = SceneManager.GetActiveScene().name.Contains("3");
        aiRacerList = FindObjectsOfType<BaseAiclass>().ToList();
    }

    private void Update()
    {
        List<RacerInfo> racerStuff = new List<RacerInfo>();

        RacerInfo playerStuff = new RacerInfo
        {
            name = "Player",
            twPoints = playerCounter.totalWaypoints,
            cwPoints = playerCounter.currentWaypoint,
            currentPosition = playerCounter.transform
        };
        racerStuff.Add(playerStuff);

        for (int i = 0; i < aiRacerList.Count; i++)
        {
            BaseAiclass aiRacer = aiRacerList[i];
            RacerInfo aiRacerData = new RacerInfo
            {
                name = "AI Racer " + (i + 1),
                twPoints = aiRacer.totalWaypoints,
                cwPoints = aiRacer.currentWaypoint,
                currentPosition = aiRacer.transform
            };
            racerStuff.Add(aiRacerData);
        }

        if (useGraph)
        {
            SortRacersGraph(racerStuff);
        }
        else
        {
            SortRacersLinkedList(racerStuff);
        }

        if (racerStuff.Count > 0)
        {
            FirstPlace = racerStuff[0].name;
        }

        if (!AreOrdersEqual(racerStuff, previousOrder))
        {
            StopAllCoroutines();
            UpdateRacerPositions(racerStuff);
            previousOrder = new List<RacerInfo>(racerStuff);
        }
    }
// decided to split it into methods, want to make it neater, one method for the graph, the other for the linked list
    private void SortRacersLinkedList(List<RacerInfo> racers)
    {
        racers.Sort((a, b) =>
        {
            int totalWaypointsCompareBS = b.twPoints.CompareTo(a.twPoints);// comparing the total waypoints
            
            if (totalWaypointsCompareBS != 0) // if it aint the same, return the number  sorts the list according to whose higher
                return totalWaypointsCompareBS;

            Transform nextA = LinkedListManager.instance.GetWaypoint(a.cwPoints);// if they did cross the same amount of waypoints, distance tie breaker
            Transform nextB = LinkedListManager.instance.GetWaypoint(b.cwPoints);// this is essentially what was in update, but a bit neater in its own method

            float distA = Vector3.Distance(a.currentPosition.position, nextA.position);
            float distB = Vector3.Distance(b.currentPosition.position, nextB.position);

            return distA.CompareTo(distB);
        });
    }

    private void SortRacersGraph(List<RacerInfo> racers)
    {
       racers.Sort((a, b) =>
    {
        int compare = b.twPoints.CompareTo(a.twPoints);
        if (compare != 0) return compare;

        int indexA = 0;
        int indexB = 0;
        Transform posA = a.currentPosition;
        Transform posB = b.currentPosition;

        float distA = float.MaxValue;
        float distB = float.MaxValue;

        //  LEADERBOARD STUFF FOR IF PLAYER IS A
        if (a.name == "Player")
        {
            indexA = playerCounter.leaderboardWaypointIndex;
            if (indexA < IgnoreBranchesManager.instance.idealGraphPath.Count)
            {
                Transform nextIdealA = IgnoreBranchesManager.instance.idealGraphPath[indexA];
                distA = Vector3.Distance(posA.position, nextIdealA.position);
            }
        }
        else
        {
            BaseAiclass aiA = posA.GetComponent<BaseAiclass>();
            if (aiA != null)
            {
                indexA = aiA.leaderboardWaypointIndex;
                if (indexA < IgnoreBranchesManager.instance.idealGraphPath.Count)
                {
                    Transform nextIdealA =IgnoreBranchesManager.instance.idealGraphPath[indexA];
                    distA = Vector3.Distance(posA.position, nextIdealA.position);
                }
            }
        }

        //  LEADERBOARD STUFF FOR IF PLAYER IS B
        if (b.name == "Player")
        {
            indexB = playerCounter.leaderboardWaypointIndex;
            if (indexB < IgnoreBranchesManager.instance.idealGraphPath.Count)
            {
                Transform nextIdealB = IgnoreBranchesManager.instance.idealGraphPath[indexB];
                distB = Vector3.Distance(posB.position, nextIdealB.position);
            }
        }
        else
        {
            BaseAiclass aiB = posB.GetComponent<BaseAiclass>();
            if (aiB != null)
            {
                indexB = aiB.leaderboardWaypointIndex;
                if (indexB < IgnoreBranchesManager.instance.idealGraphPath.Count)
                {
                    Transform nextIdealB = IgnoreBranchesManager.instance.idealGraphPath[indexB];
                    distB = Vector3.Distance(posB.position, nextIdealB.position);
                }
            }
        }

// COMPARE THE IDEAL PATH, IF SOMEONE IS AHEAD FIRST, IF NOT  THEN DO DISTANCE, THIS IS JUST FOR ACCURACY’S SAKE, IT IS NOT NEEDED, I CAN JUST HAVE THEM RETURN THE COMPARE DISTANCE BUT I WANT TO MAKE IT AS ACCURATE AS POSSIBLE
        int idealCompare = indexB.CompareTo(indexA);
        if (idealCompare != 0) return idealCompare;

        
        return distA.CompareTo(distB); // IM GONNA KILL MYSELF T-T
    });
}

    private void UpdateRacerPositions(List<RacerInfo> racerStuff)
    {
        for (int i = 0; i < leaderboardTexts.Count; i++)
        {
            if (i < racerStuff.Count)
            {
                string newText = (i + 1) + ". " + racerStuff[i].name;
                if (leaderboardTexts[i].text != newText)
                {
//                    Debug.Log("Animation Should Play");
                    StartCoroutine(TypewriterEffect(leaderboardTexts[i], newText));
                }
            }
            else
            {
                leaderboardTexts[i].text = "";
            }
        }
    }

    bool AreOrdersEqual(List<RacerInfo> currentOrder, List<RacerInfo> previousOrder)
    {
        if (currentOrder.Count != previousOrder.Count)
            return false;

        for (int i = 0; i < currentOrder.Count; i++)
        {
            if (currentOrder[i].twPoints != previousOrder[i].twPoints) return false;
            if (currentOrder[i].cwPoints != previousOrder[i].cwPoints) return false;

            if (Vector3.Distance(currentOrder[i].currentPosition.position, previousOrder[i].currentPosition.position) != 0)
                return false;
        }

        return true;
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textMesh, string newText)
    {
        textMesh.text = "";
        for (int i = 0; i < newText.Length; i++)
        {
            textMesh.text += newText[i];
            yield return new WaitForSeconds(0.01f);
        }
    }
}

public class RacerInfo
{
    public string name;
    public int twPoints;
    public int cwPoints;
    public Transform currentPosition;
}