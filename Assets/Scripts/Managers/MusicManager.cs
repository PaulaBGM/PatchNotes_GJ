using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float crossfadeDuration = 1f;

    [Header("Initial Music")]
    [SerializeField] private AudioClip initialMusic; // opcional: música del menú

    private AudioSource currentSource;
    private AudioSource nextSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentSource = gameObject.AddComponent<AudioSource>();
        nextSource = gameObject.AddComponent<AudioSource>();

        currentSource.loop = true;
        nextSource.loop = true;

        currentSource.volume = 1f;
        nextSource.volume = 1f;

        // Reproducir música inicial si se asignó
        if (initialMusic != null)
        {
            currentSource.clip = initialMusic;
            currentSource.Play();
        }
    }

    /// <summary>
    /// Reproduce un clip con crossfade.
    /// </summary>
    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        // Ignorar si el mismo clip ya se está reproduciendo con volumen > 0
        if (currentSource.isPlaying && currentSource.clip == clip && currentSource.volume > 0f)
            return;

        StopAllCoroutines();
        StartCoroutine(CrossfadeTo(clip, volume));
    }

    private IEnumerator CrossfadeTo(AudioClip newClip, float targetVolume)
    {
        // Si no hay clip actualmente, reproducir directamente
        if (currentSource.clip == null)
        {
            currentSource.clip = newClip;
            currentSource.volume = targetVolume;
            currentSource.Play();
            yield break;
        }

        nextSource.clip = newClip;
        nextSource.volume = 0f;
        nextSource.Play();

        float startVolume = currentSource.volume;
        float timer = 0f;

        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeDuration;

            currentSource.volume = Mathf.Lerp(startVolume, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, targetVolume, t);

            yield return null;
        }

        currentSource.volume = 0f;
        nextSource.volume = targetVolume;

        // Intercambiar referencias
        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
        nextSource.Stop();
    }

    /// <summary>
    /// Ajusta el volumen global de la música.
    /// </summary>
    public void SetGlobalVolume(float volume)
    {
        currentSource.volume = volume;
        nextSource.volume = volume;
    }
}
