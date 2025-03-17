using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChoices", menuName = "Scriptable Objects/DialogueChoices")]
public class DialogueChoices : ScriptableObject
{
    [Header("Lists")]
    public List<string> choices; 
    [Space]
    public List<NPCDialogue> outcomes;
}
