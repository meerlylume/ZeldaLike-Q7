using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPCDialogue")]

public class NPCDialogue : ScriptableObject
{

    public string npcName;
    //public sprite npcSprite;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float talkingSpeed = 0.05f;

}

//putting this here in case I wanna continue later .https://youtu.be/eSH9mzcMRqw?si=eJ-Il9ZZSRjT210S