using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class StackManager : MonoBehaviour
{
    [SerializeField] private List<Trigger> TriggerList = new List<Trigger>();
    public static StackManager Instance;
    private Stackimplement<Trigger> checkPoint = new Stackimplement<Trigger>();
    private float TimeAdded = 5f;
    public delegate void AddSecondsToTimer(float seconds);
    public static event AddSecondsToTimer OnAddSecondsToTimer;
    public delegate void WinGameCondition();
    public static event WinGameCondition OnWinGameCondition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CrossCheckPoint(Trigger trigger)
    {
        if (checkPoint.isEmpty() == false && checkPoint.Peek() == trigger)
        {
            trigger.Visted();
            checkPoint.Pop();
            OnAddSecondsToTimer?.Invoke(TimeAdded);
           Debug.Log("Checking if stack is empty");
            Debug.Log(checkPoint.isEmpty());
           if (checkPoint.isEmpty())
            {
                Debug.Log("race over");
                OnWinGameCondition?.Invoke();
                //StopAllCoroutines();
                return;
            }
            checkPoint.Peek().TargetColor();
        }
    }
    void Start()
    {
        TriggerList = FindObjectsOfType<Trigger>().OrderBy(c => c.ID).ToList();
        for (int i = TriggerList.Count-1; i >= 0; i--)
        {
            checkPoint.Push(TriggerList[i]);
           TriggerList[i].Remaining();
        }
        if (checkPoint.isEmpty() == false)
        {
            Debug.Log("checking for peek, this should only happen once");
            checkPoint.Peek().TargetColor();
        }
    }
}
