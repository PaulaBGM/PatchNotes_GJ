using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float crossfadeDuration = 1f;

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
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        if (currentSource.isPlaying && currentSource.clip == clip)
            return;

        StopAllCoroutines();
        StartCoroutine(CrossfadeTo(clip, volume));
    }

    private IEnumerator CrossfadeTo(AudioClip newClip, float targetVolume)
    {
        nextSource.clip = newClip;
        nextSource.volume = 0f;
        nextSource.Play();

        float timer = 0f;

        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeDuration;

            currentSource.volume = Mathf.Lerp(targetVolume, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, targetVolume, t);

            yield return null;
        }

        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
        nextSource.Stop();
    }
    public void SetGlobalVolume(float volume)
    {
        currentSource.volume = volume;
        nextSource.volume = volume;
    }

}
