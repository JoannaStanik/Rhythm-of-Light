using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Header("ród³o muzyki (AudioSource)")]
    public AudioSource musicSource;

    public float MusicTime => (musicSource && musicSource.clip) ? musicSource.time : 0f;
    public float MusicLength => (musicSource && musicSource.clip) ? musicSource.clip.length : 0f;

    void Reset()
    {
        musicSource = GetComponent<AudioSource>();
        if (musicSource) musicSource.playOnAwake = false;
    }

    public void Play()
    {
        if (musicSource && musicSource.clip) musicSource.Play();
    }

    public void Stop()
    {
        if (musicSource) musicSource.Stop();
    }
}
