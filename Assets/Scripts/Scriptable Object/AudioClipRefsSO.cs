using UnityEngine;

[CreateAssetMenu(fileName = "Audio References", menuName = "Scriptable Object/Audio Reference")]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFailed;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip panSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;
}
