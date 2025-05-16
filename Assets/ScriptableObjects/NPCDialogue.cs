using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]

public class NPCDialogue : ScriptableObject
{
    [Header("Identity")]
    public Identity identity;
    public bool isPortraitOnTheRight = false; [Space]

    [Header("Speech Variables")]
    public float talkingSpeed = 0.02f; [Space]
    //talking sound & pitch goes here

    [Header("Dialogue")]
    public string[] dialogueLines; [Space]
    public DialogueChoices dialogueChoices; [Space]
    public bool questTrigger = false;
    public NPCDialogue nextDialogue;

    [Header("Items")]
    public bool giveItems = false;
    public InventoryData itemsToGive;

    //Dialogue flags?
}
