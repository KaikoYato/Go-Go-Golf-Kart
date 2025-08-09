using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "DialoguescriptableObject", menuName = "Scriptable Objects/DialoguescriptableObject")]
public class DialoguescriptableObject : ScriptableObject
{
    public string SpeakerName;
    public Sprite sprite;
    public string dialogueText;
}