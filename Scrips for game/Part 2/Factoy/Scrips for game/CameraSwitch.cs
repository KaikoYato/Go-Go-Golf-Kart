using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] cams;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    [SerializeField] private CinemachineVirtualCamera StartCamera;
    private CinemachineVirtualCamera CurrentCamera;
    private Transform ReverseCameraLookAt;
    private Transform CameraLookAtAndFollow;
    private void Awake()
    {
    GameObject MainCameraPov = GameObject.FindGameObjectWithTag("PlayerMesh");
    ReverseCameraLookAt = MainCameraPov.transform.Find("CameraFollow");
    CameraLookAtAndFollow = MainCameraPov.transform.Find("The Car");
    cam1.Follow = CameraLookAtAndFollow;
    cam1.LookAt = CameraLookAtAndFollow;
    cam2.Follow = ReverseCameraLookAt;
    cam2.LookAt = CameraLookAtAndFollow;
    }
    void Start()
    {
        CurrentCamera = StartCamera;
        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i] == CurrentCamera)
            {
                Debug.Log("this is setting it to 20");
                cams[i].Priority = 20;
            }
            else
            {
                cams[i].Priority = 10;
            }
        }
    }

    public void switchCam(CinemachineVirtualCamera newCam)
    {
        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].Priority = 10;
        }
        CurrentCamera = newCam;
        CurrentCamera.Priority = 20;
    }
    
}
