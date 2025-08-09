using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    private HashMap<string, AudioClip> audioStuff;

    private AudioSource noLoopsfx;// for non background track

    private AudioSource loopSfx;// for background track


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        noLoopsfx = gameObject.AddComponent<AudioSource>();
        loopSfx = gameObject.AddComponent<AudioSource>();
        loopSfx.loop = true;
        audioStuff = new HashMap<string, AudioClip>();
        AddtoHash();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    private void AddtoHash()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip == null)
            {
                Debug.LogWarning("Daar is Niks"); // safety check
            } 
            audioStuff.Add(clip.name, clip);// uses the name as the key
        }
    }

    public void playRequestedSound(string clipName, bool isLoop = false)
    {// this method would be called wherever, it will take the string, search the hashMap and then if its in the hashmap it will play
        AudioClip clip = audioStuff.Find(clipName);
        if (isLoop)
        {
            if (loopSfx.clip == clip && loopSfx.isPlaying)
            {
                Debug.LogWarning("Naur this thing is already playing");
                return; // if it is playing already, it will return
            }
            
            loopSfx.clip = clip;
            loopSfx.loop = true;
            loopSfx.Play();
        }
        else
        {
            noLoopsfx.PlayOneShot(clip);
        }
    }

    public void stopSound()
    {
        loopSfx.Stop();
        noLoopsfx.Stop();
    }
}
