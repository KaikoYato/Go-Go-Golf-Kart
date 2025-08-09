using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialoguescriptableObject CurrentDialogue;
    [SerializeField] private TextMeshProUGUI SpeakerName;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private HolderDialogue sceneDialogue;
    private int indexListTracker;
    private bool isTyping;
    private DialogueQueue<DialoguescriptableObject> dialogueQueue = new DialogueQueue<DialoguescriptableObject>();
    [SerializeField] private UnityEngine.UI.Image PortraitImage;
    [SerializeField] private AudioClip SpeakerOneEffect;
    [SerializeField] private AudioClip SpeakerTwoEffect;
    [SerializeField] private AudioSource audioSource;
    private string CurrentScene;
    private string CHPR = "CheckPointDialogue";
    private string BAR = "B-DIALOGUE";
    private string AAR = "AD-DIALOGUE";
    private void Awake()
    {
        EnqueueDialogue();
    }

    private void EnqueueDialogue()
    {
        for (int i = 0; i < sceneDialogue.Script.Count; i++)
        {
            dialogueQueue.Enqueue(sceneDialogue.Script[i]);
        }
    }
    void Start()
    {
        DisplayDialogue();
        CurrentScene = SceneManager.GetActiveScene().name;
    }
    public void DisplayDialogue()
    {
        if (dialogueQueue.isEmpty() == false)
        {
            CurrentDialogue = dialogueQueue.Dequeue();
            SpeakerName.text = CurrentDialogue.SpeakerName;
            if (CurrentDialogue.SpeakerName == "John")
            {
                audioSource.PlayOneShot(SpeakerOneEffect);
            }
            else if (CurrentDialogue.SpeakerName == "Dan")
            {
                audioSource.PlayOneShot(SpeakerTwoEffect);
            }

            StartCoroutine(TypeWritterEffect(CurrentDialogue.dialogueText));
            PortraitImage.sprite = CurrentDialogue.sprite;
        }
    }
    public void NextDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
            DialogueText.text = CurrentDialogue.dialogueText;
            isTyping = false;
            Debug.Log("testing if this works");
            Debug.Log(isTyping);
            return;
        }
        if (!dialogueQueue.isEmpty() && isTyping == false)
        {
            DisplayDialogue();
        }
        else if (dialogueQueue.isEmpty())
        {
            Debug.Log("switchingScene");
            if (CurrentScene == CHPR)
            {
                Debug.Log("cpr");
                SceneManager.LoadScene("CheckPointRace1");
            }

            if (CurrentScene == BAR)
            {
                Debug.Log("bar");
                SceneManager.LoadScene("B-RACE2");
            }

            if (CurrentScene == AAR)
            {
                Debug.Log("aar");
                SceneManager.LoadScene("AD-RACE3");
            }
        }

    }
    private IEnumerator TypeWritterEffect(string spokenText)
    {
        isTyping = true;
        DialogueText.text = "";
        for (int i = 0; i < spokenText.Length; i++)
        {
            DialogueText.text += spokenText[i];
            yield return new WaitForSeconds(0.1f);
        }
        isTyping = false;
    }
}
