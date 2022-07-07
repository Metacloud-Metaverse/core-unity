using UnityEngine;

[CreateAssetMenu(fileName = "Platform Settings", menuName = "ScriptableObject/Environment/PlatformSettings")]
public class PlatformSettings : ScriptableObject
{
    public float time = 1;
    public float delayOnStartRached = 1;
    public float delayOnEndRached = 1;
    public iTween.EaseType ease;
}
