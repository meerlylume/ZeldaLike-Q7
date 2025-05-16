using UnityEngine;

[CreateAssetMenu(fileName = "Identity", menuName = "Scriptable Objects/Identity")]
public class Identity : ScriptableObject
{
    public new string name;
    public Sprite portrait;
}
