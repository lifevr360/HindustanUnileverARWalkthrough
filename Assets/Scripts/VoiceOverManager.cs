using UnityEngine;

public class VoiceOverManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void PlayVoice(AudioClip clip)
    {
        if (clip == null)
            return;

        // Stop the currently playing audio
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopVoice()
    {
        audioSource.Stop();
    }
}