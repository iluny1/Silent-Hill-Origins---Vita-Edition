using UnityEngine;

public class SoundFXBank : MonoBehaviour
{
    public AudioClip[] clips;

    public AudioClip GetClip(int index)
    {
        if (index < 0 || index >= clips.Length)
            return null;
        else
            return clips[index];
    }
}
