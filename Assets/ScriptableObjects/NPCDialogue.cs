using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]

public class NPCDialogue : ScriptableObject
{
    [Header("Identity")]
    public string npcName;
    public Sprite npcSprite;
    public Sprite npcPortrait; 
    public bool   isPortraitOnTheRight = true; [Space]

    [Header("Speech Variables")]
    public float talkingSpeed = 0.25f; [Space]
    //talking sound & pitch goes here

    [Header("Lines of Dialogue")]
    public string[] dialogueLines;
}
