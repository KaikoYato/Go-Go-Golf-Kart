using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject WinningScreen;
    [SerializeField] private GameObject LosingScreen;
    [SerializeField] private GameObject RestartButton;
    
    private bool level1;
    private bool level2;
    private bool level3;

    private void Awake()
    {
        level3 = SceneManager.GetActiveScene().name.Contains("3");
        level2 = SceneManager.GetActiveScene().name.Contains("2");
        level1 = SceneManager.GetActiveScene().name.Contains("1");
    }

    private void Start()
    {
        PlayBackTrack();
    }

    private void PlayBackTrack()
    {
        if (level3 || level2)
        {
            SFXManager.instance.playRequestedSound("Track2&3", isLoop: true);
            Debug.Log(level3);
        }
        else if (level1)
        {
            SFXManager.instance.playRequestedSound("TrackOne", isLoop: true);
        }
    }
    private void DisplayWinningScreen()
    {
        WinningScreen.SetActive(true);
        RestartButton.SetActive(false);
        SFXManager.instance.stopSound();
        SFXManager.instance.playRequestedSound("WinScreen", isLoop: true);
    }

    public void DisplayLosingScreen()
    {
        LosingScreen.SetActive(true);
        RestartButton.SetActive(false);
        SFXManager.instance.stopSound();
        SFXManager.instance.playRequestedSound("GameOver", isLoop: true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnEnable()
    {
        StackManager.OnWinGameCondition += DisplayWinningScreen;
        RaceAndLapManager.OnWinGameCondition += DisplayWinningScreen;
        RaceAndLapManager.OnLoseGameCondition += DisplayLosingScreen;
        TimerScript.OnLoseGameCondition += DisplayLosingScreen;
    }

    private void OnDisable()
    {
        RaceAndLapManager.OnWinGameCondition -= DisplayWinningScreen;
        RaceAndLapManager.OnLoseGameCondition -= DisplayLosingScreen;
        TimerScript.OnLoseGameCondition -= DisplayLosingScreen;
        StackManager.OnWinGameCondition -= DisplayWinningScreen;
    }
}
