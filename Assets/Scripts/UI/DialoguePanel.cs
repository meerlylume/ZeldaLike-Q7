using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    //not gonna lie, this script exists purely because I keep forgetting to
    //deactivate this gameobject in the inspector whenever I make changes

    void Start() { gameObject.SetActive(false); }
}
