using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Crash Audio Clips")]
    public AudioClip crashClip1;
    public AudioClip crashClip2;
    [Range(0f, 1f)] public float crashVolume = 1f;

    [Header("UI Audio Clips")]
    public AudioClip buttonClickClip;
    [Range(0f, 1f)] public float buttonVolume = 1f;

    [Header("Pitch Randomization")]
    [Range(0.5f, 1.5f)] public float minPitch = 0.9f;
    [Range(0.5f, 1.5f)] public float maxPitch = 1.1f;

    private readonly List<AudioSource> audioSources = new();
    private readonly HashSet<Button> registeredButtons = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource initialSource = GetComponent<AudioSource>();
        initialSource.playOnAwake = false;
        audioSources.Add(initialSource);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        RegisterAllButtons();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterAllButtons();
    }

    public void RegisterAllButtons()
    {
        registeredButtons.RemoveWhere(btn => btn == null);
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsInactive.Include);

        foreach (Button btn in allButtons)
        {
            if (registeredButtons.Add(btn))
            {
                btn.onClick.AddListener(PlayButtonClick);
            }
        }
    }

    public void PlayCrashSounds()
    {
        PlayClipRandomPitch(crashClip1, crashVolume);
        PlayClipRandomPitch(crashClip2, crashVolume);
    }

    public void PlayButtonClick()
    {
        PlayClipRandomPitch(buttonClickClip, buttonVolume);
    }

    private void PlayClipRandomPitch(AudioClip clip, float volume)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableAudioSource();
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(clip, volume);
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        audioSources.Add(newSource);
        return newSource;
    }
}