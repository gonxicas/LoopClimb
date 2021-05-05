using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class InGameOptions : MonoBehaviour
{
    public static InGameOptions Instance;
    
    [SerializeField] public Slider music;
    [SerializeField] private Slider master;
    [SerializeField] private Slider soundEffects;
    [SerializeField] private GameObject pausePanel;

    private bool _soundEffectPlaying = false;
    public GameObject PausePanel => pausePanel;

    private void Awake()
    {
        music.onValueChanged.AddListener(SetMusicLevel);
        master.onValueChanged.AddListener(SetMasterLevel);
        soundEffects.onValueChanged.AddListener(SetSoundEffectsLevel);
        Instance = this;
    }

    private void Start()
    {
        music.value = SoundManagaer.instance.MusicVolume;
        master.value = SoundManagaer.instance.MasterVolume;
        soundEffects.value = SoundManagaer.instance.SoundEffectsVolume;
    }

    private void Update()
    {
        CheckPausePanel();
    }

    private void CheckPausePanel()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void SetMusicLevel(float value)
    {
        SoundManagaer.instance.Mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        SoundManagaer.instance.MusicVolume = value;
    }
    private void SetMasterLevel(float value)
    {
        if (!_soundEffectPlaying)
            StartCoroutine(PlayJumpSound());
        SoundManagaer.instance.Mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        SoundManagaer.instance.MasterVolume = value;
    }
    private void SetSoundEffectsLevel(float value)
    {
        if (!_soundEffectPlaying)
            StartCoroutine(PlayJumpSound());
        SoundManagaer.instance.Mixer.SetFloat("SoundEffectsVolume", Mathf.Log10(value) * 20);
        SoundManagaer.instance.SoundEffectsVolume = value;
    }

    IEnumerator PlayJumpSound()
    {
        _soundEffectPlaying = true;
        SoundManagaer.instance.PlayJump();
        yield return new WaitForSecondsRealtime(0.5f);
        _soundEffectPlaying = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
