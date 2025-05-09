using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]

public class NPCDialogue : ScriptableObject
{
    [Header("Identity")]
    public new string name;
    public Sprite npcSprite;
    public Sprite npcPortrait; 
    public bool   isPortraitOnTheRight = true; [Space]

    [Header("Speech Variables")]
    public float talkingSpeed = 0.25f; [Space]
    //talking sound & pitch goes here

    [Header("Dialogue")]
    public string[] dialogueLines; [Space]
    public DialogueChoices dialogueChoices; [Space]
    public bool questTrigger = false;

    [Header("Items")]
    public bool giveItems = false;
    public InventoryData itemsToGive;

    //Dialogue flags?
}
