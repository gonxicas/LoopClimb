using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManagaer : MonoBehaviour
{
    public static SoundManagaer instance;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip fall;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
        _musicVolume = 0.7f;
        _masterVolume = 0.7f;
        _soundEffectsVolume = 0.7f;
    }
    private void Update()
    {
        mixer.SetFloat("SoundEffectsPitch", Random.Range(minPitch, maxPitch));
    }

    public void PlayJump() => _audioSource.PlayOneShot(jump);
    public void PlayFall() => _audioSource.PlayOneShot(fall);

    #region MIXER

    public AudioMixer Mixer => mixer;


    public float MusicVolume
    {
        get => _musicVolume;
        set => _musicVolume = value;
    }

    public float MasterVolume
    {
        get => _masterVolume;
        set => _masterVolume = value;
    }

    public float SoundEffectsVolume
    {
        get => _soundEffectsVolume;
        set => _soundEffectsVolume = value;
    }

    private float _musicVolume;
    private float _masterVolume;
    private float _soundEffectsVolume;

    

   

   

    #endregion
}



