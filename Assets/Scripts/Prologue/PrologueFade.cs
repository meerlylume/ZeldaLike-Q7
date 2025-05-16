using UnityEngine;

public class PrologueFade : MonoBehaviour
{
    public void OnFadeEnd() { GameSaver.Instance.LoadGame(); }
}
