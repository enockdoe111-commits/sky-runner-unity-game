// AudioManager.cs - Centralized audio management system

using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float sfxVolume = 1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Setup audio sources
        AudioSource[] audioSources = GetComponents<AudioSource>();
        
        if (audioSources.Length < 2)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            musicSource = audioSources[0];
            sfxSource = audioSources[1];
        }

        // Configure music source
        musicSource.loop = true;
        musicSource.volume = musicVolume * masterVolume;
        musicSource.clip = backgroundMusic;

        // Configure SFX source
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume * masterVolume;

        // Initialize SFX dictionary with common sounds
        InitializeSFXClips();
    }

    private void InitializeSFXClips()
    {
        // These will be populated from resources or inspector
        sfxClips["jump"] = Resources.Load<AudioClip>("Audio/SFX/jump");
        sfxClips["coin"] = Resources.Load<AudioClip>("Audio/SFX/coin");
        sfxClips["powerUp"] = Resources.Load<AudioClip>("Audio/SFX/powerUp");
        sfxClips["collision"] = Resources.Load<AudioClip>("Audio/SFX/collision");
        sfxClips["gameOver"] = Resources.Load<AudioClip>("Audio/SFX/gameOver");
        sfxClips["slide"] = Resources.Load<AudioClip>("Audio/SFX/slide");
        sfxClips["shieldBreak"] = Resources.Load<AudioClip>("Audio/SFX/shieldBreak");
        sfxClips["button"] = Resources.Load<AudioClip>("Audio/SFX/button");
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource.isPlaying) return;
        musicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void PauseBackgroundMusic()
    {
        musicSource.Pause();
    }

    public void ResumeBackgroundMusic()
    {
        musicSource.UnPause();
    }

    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (!sfxClips.ContainsKey(clipName))
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
            return;
        }

        sfxSource.PlayOneShot(sfxClips[clipName], volume * sfxVolume * masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume * masterVolume;
        if (sfxSource != null)
            sfxSource.volume = sfxVolume * masterVolume;
    }
}
