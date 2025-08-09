using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launchtimer : MonoBehaviour
{
    
    [SerializeField]private float timer = 5f;
    private bool timerActive = false;
    public TextMeshProUGUI TimerUi;
    public List<BaseAiclass> aiRacers;
    private CarController player;
    private bool isLevel3;
   
    private void Awake()
    {
        
        isLevel3 = SceneManager.GetActiveScene().name.Contains("3");
        timerActive = true;
    }

    void Start()
    {
        
        aiRacers = new List<BaseAiclass>(FindObjectsOfType<BaseAiclass>());
        player = FindObjectOfType<CarController>();
        Debug.Log("please excute i beg");
        Debug.Log(timer);
        StartCoroutine(Countdown());
        updateUi();
    }
    
    private void updateUi()
    {
        int mins = Mathf.FloorToInt(timer / 60);
        int secs = Mathf.FloorToInt(timer % 60);
        TimerUi.text = string.Format("{0:00}:{1:00}", mins, secs);
    }
    
    private IEnumerator Countdown()
    {
        while (timer > 0 && timerActive)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            updateUi();
        }
        if (timer <= 0)
        {
            timerActive = false;
            TimerUi.text = "";
            player.canMove = true; // to allow the player to move
            if (!isLevel3)
            {
                for (int i = 0; i < aiRacers.Count; i++)
                {
                    aiRacers[i].SetNextDestination(); // sets canmove to true and calls the next destination for all the ais to it can move
                }
            }

            else if (isLevel3)
            {
                for (int i = 0; i < aiRacers.Count; i++)
                {
                    aiRacers[i].SetNextDestinaionGraph();// sets canmove to true and calls the next destination for all the ais to it can move
                }
            }
           
            
        }
    }
}
