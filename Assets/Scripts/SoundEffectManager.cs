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

    //for UI
    //music
    [SerializeField] private Image muteButtonImage;
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite unmuteSprite;
    [SerializeField] private Image volumeBarImage;
    [SerializeField] private Sprite[] volumeBarSprites;
    //sfx
    [SerializeField] private Image sfxVolumeBarImage;
    [SerializeField] private Image sfxMuteButtonImage;

    private float lastSfxVolume = 1f;
    private bool isSfxMuted = false;

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

        if(musicMuteToggle != null)
            musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToggled);
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
        instance.randomPitchAudioSource.volume = volume;
    }

    public void OnVolumeChanged()
    {
        if (isSfxMuted && sfxSlider.value > 0f)
        {
            isSfxMuted = false;
        }

        lastSfxVolume = sfxSlider.value;

        SetVolume(sfxSlider.value);

        UpdateSfxUI();
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

        int index = Mathf.RoundToInt(musicSlider.value * 10);
        volumeBarImage.sprite = volumeBarSprites[index];
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
        int index = Mathf.RoundToInt(musicSlider.value * 10);
        volumeBarImage.sprite = volumeBarSprites[index];

        muteButtonImage.sprite = isMusicMuted ? unmuteSprite : muteSprite;
    }
    public void IncreaseMusicVolume()
    {
        musicSlider.value = Mathf.Clamp01(musicSlider.value + 0.1f);
    }

    public void DecreaseMusicVolume()
    {
        musicSlider.value = Mathf.Clamp01(musicSlider.value - 0.1f);
    }

    public void ToggleMusicMute()
    {
        OnMusicMuteToggled(!isMusicMuted);
    }

    public void IncreaseSfxVolume()
    {
        sfxSlider.value = Mathf.Clamp01(sfxSlider.value + 0.1f);
    }

    public void DecreaseSfxVolume()
    {
        sfxSlider.value = Mathf.Clamp01(sfxSlider.value - 0.1f);
    }

    public void ToggleSfxMute()
    {
        isSfxMuted = !isSfxMuted;

        if (isSfxMuted)
        {
            int index = Mathf.RoundToInt(sfxSlider.value * 10);
            sfxVolumeBarImage.sprite = volumeBarSprites[index];

            lastSfxVolume = sfxSlider.value > 0 ? sfxSlider.value : lastSfxVolume;

            SetVolume(0f);
            sfxSlider.SetValueWithoutNotify(0f);
        }
        else
        {
            float volume = lastSfxVolume > 0 ? lastSfxVolume : 1f;

            SetVolume(volume);
            sfxSlider.SetValueWithoutNotify(volume);
        }

        UpdateSfxUI();
    }

    private void UpdateSfxUI()
    {
        int index = Mathf.RoundToInt(sfxSlider.value * 10);

        sfxVolumeBarImage.sprite = volumeBarSprites[index];

        sfxMuteButtonImage.sprite =
            isSfxMuted ? unmuteSprite : muteSprite;
    }
    public List<float> GetSoundSettings()
    {
        return new List<float> { sfxSlider.value, musicSlider.value, isMusicMuted ? 1f : 0f };
    }
    public void LoadSoundSettings(float sfxVolume, float musicVolume, bool isMuted)
    {
        sfxSlider.SetValueWithoutNotify(sfxVolume);
        musicSlider.SetValueWithoutNotify(musicVolume);
        musicMuteToggle.SetIsOnWithoutNotify(isMuted);

        SetVolume(sfxVolume);
        SetMusicVolume(isMuted ? 0f : musicVolume);
        isMusicMuted = isMuted;
    }
}