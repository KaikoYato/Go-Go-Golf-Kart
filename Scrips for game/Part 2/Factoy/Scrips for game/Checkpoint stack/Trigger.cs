using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Trigger : MonoBehaviour
{
    private Renderer colorRenderer;
    public int ID;// so the checkpoints are in order
    private Color TargetCheckPointColor = Color.green;
    private Color CrossedCheckPointColor = Color.red;
    private Color UpcomingCheckPointColor = Color.yellow;
     [SerializeField] private List<Renderer> CheckPointColumns = new List<Renderer>();
    private void Awake()
    {
        CheckPointColumns.AddRange(GetComponentsInChildren<Renderer>());
    }
    public void TargetColor()
    {
        for (int i = 0; i < CheckPointColumns.Count; i++)
        {
            CheckPointColumns[i].material.color = TargetCheckPointColor;
        }
    }
    public void Visted()
    { 
        for (int i = 0; i < CheckPointColumns.Count; i++)
        {
            CheckPointColumns[i].material.color = CrossedCheckPointColor;
        }
        SFXManager.instance.playRequestedSound("CheckPoint", isLoop: false);
    }
    public void Remaining()
    {
        for (int i = 0; i < CheckPointColumns.Count; i++)
        {
            CheckPointColumns[i].material.color = UpcomingCheckPointColor;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            StackManager.Instance.CrossCheckPoint(this);
        }
    }
    
}
