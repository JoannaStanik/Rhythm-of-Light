using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    public float MusicTime => (musicSource && musicSource.clip) ? musicSource.time : 0f;
    public float MusicLength => (musicSource && musicSource.clip) ? musicSource.clip.length : 0f;

    void Awake()
    {
        if (!musicSource) musicSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (musicSource && musicSource.clip)
            musicSource.Play();
    }

    public void Stop()
    {
        if (musicSource) musicSource.Stop();
    }

    public void SetClip(AudioClip clip)
    {
        musicSource.clip = clip;
    }
}
