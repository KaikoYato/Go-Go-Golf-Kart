using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField]private float timer = 30f;
    private bool timerActive = false;
    public TextMeshProUGUI TimerUi;
    public delegate void LoseGameCondition();
    public static event LoseGameCondition OnLoseGameCondition;
    private void Awake()
    {
        timerActive = true;
    }
    void Start()
    {
        StartCoroutine(Countdown());
        updateUi();
    }
    private void OnEnable()
    {
        StackManager.OnAddSecondsToTimer += addTime;
        StackManager.OnWinGameCondition += StopTimer;
    }
    private void OnDisable()
    {
        StackManager.OnAddSecondsToTimer -= addTime;
        StackManager.OnWinGameCondition -= StopTimer;
    }
    public void addTime(float _time)
    {
        timer += _time;
        updateUi();
    }
    public void StopTimer()
    {
        StopCoroutine(Countdown());
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
           OnLoseGameCondition?.Invoke();
        }
    }
}

