using System;
using Scrips_for_game.Part_2_shit.Factoy_stuff;
using UnityEngine;

public class FactioryManager : MonoBehaviour
{
    public Abstact_Racer_Factory RacerFactory;
    
    public Transform spawnLocation1;
    public Transform spawnLocation2;
    public Transform spawnLocation3;
    
    public Transform spawnLocation4;
    public Transform spawnLocation5;
    public Transform spawnLocation6;
    
    public Transform spawnLocation7;
    public Transform spawnLocation8;
    private void Awake()
    {
        // calls the concrete factory stuff
        Vector3 spawnPoint1 = spawnLocation1.position;
        RacerFactory.CreateRacer3(spawnPoint1);
        Vector3 spawnPoint2 = spawnLocation2.position;
        RacerFactory.CreateRacer3(spawnPoint2);
        Vector3 spawnPoint3 = spawnLocation3.position;
        RacerFactory.CreateRacer3(spawnPoint3);
        
        Vector3 spawnPoint4 = spawnLocation4.position;
        RacerFactory.CreateRacer2(spawnPoint4);
        Vector3 spawnPoint5 = spawnLocation5.position;
        RacerFactory.CreateRacer2(spawnPoint5);
        Vector3 spawnPoint6 = spawnLocation6.position;
        RacerFactory.CreateRacer2(spawnPoint6);
        
        Vector3 spawnPoint7 = spawnLocation7.position;
        RacerFactory.CreateRacer1(spawnPoint7);
        Vector3 spawnPoint8 = spawnLocation8.position;
        RacerFactory.CreateRacer1(spawnPoint8);
        
    }
}
