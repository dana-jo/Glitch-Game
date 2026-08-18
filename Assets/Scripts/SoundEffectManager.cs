using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    private AudioSource audioSource;
    private AudioSource randomPitchAudioSource;
    private AudioSource musicSource;
    private SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle musicMuteToggle;
    [SerializeField] private AudioClip defaultMusicTrack;
    private float lastMusicVolume = 1f;
    private bool isMusicMuted = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource =audioSources[0];
            randomPitchAudioSource = audioSources[1];
            musicSource  = audioSources[2];
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            //DontDestroyOnLoad(gameObject);

            if(defaultMusicTrack != null)
                PlayMusic(defaultMusicTrack);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static void play(string soundName, bool randomPitch = false)
    {
        AudioClip audioClip = instance.soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            if(randomPitch)
            {
                instance.randomPitchAudioSource.pitch = Random.Range(0.5f, 0.8f);
                instance.randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
            {
                instance.audioSource.PlayOneShot(audioClip);    
            }
        }
    }
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToggled);
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
        instance.randomPitchAudioSource.volume = volume;
    }

    public void OnVolumeChanged()
    {
        SetVolume(sfxSlider.value);
    }

    public static void PlayMusic(AudioClip clip)
    {
        if (instance.musicSource.clip != clip)
        {
            instance.musicSource.clip = clip;
            instance.musicSource.Play();
        }
    }

    public static void StopMusic()
    {
        instance.musicSource.Stop();
    }

    public static void SetMusicVolume(float volume)
    {
        instance.musicSource.volume = volume;
    }

    public void OnMusicVolumeChanged()
    {
        if (isMusicMuted && musicSlider.value > 0f)
        {
            isMusicMuted = false;
            musicMuteToggle.SetIsOnWithoutNotify(false);
        }
        lastMusicVolume = musicSlider.value;
        SetMusicVolume(musicSlider.value);
    }
    public void OnMusicMuteToggled(bool isMuted)
    {
        isMusicMuted = isMuted;
        if (isMuted)
        {
            lastMusicVolume = musicSlider.value > 0 ? musicSlider.value : lastMusicVolume;
            SetMusicVolume(0f);
             musicSlider.SetValueWithoutNotify(0f);
        }
        else
        {
            SetMusicVolume(lastMusicVolume > 0 ? lastMusicVolume : 1f);
            musicSlider.SetValueWithoutNotify(lastMusicVolume > 0 ? lastMusicVolume : 1f);
        }
    }
}