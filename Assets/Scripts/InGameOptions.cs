using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace LoopClimb.Others
{
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
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            music.onValueChanged.AddListener(SetMusicLevel);
            master.onValueChanged.AddListener(SetMasterLevel);
            soundEffects.onValueChanged.AddListener(SetSoundEffectsLevel);
        }

        private void Start()
        {
            music.value = SoundManagaer.Instance.MusicVolume;
            master.value = SoundManagaer.Instance.MasterVolume;
            soundEffects.value = SoundManagaer.Instance.SoundEffectsVolume;
        }

        private void Update()
        {
            CheckPausePanel();
        }
        //Checks if the pause panel is active and pauses the game.
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
            SoundManagaer.Instance.Mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
            SoundManagaer.Instance.MusicVolume = value;
        }
        
        private void SetMasterLevel(float value)
        {  
            //Plays a jump sound effect to check the volume of the sfx
            if (!_soundEffectPlaying)
                StartCoroutine(PlayJumpSound());
            SoundManagaer.Instance.Mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
            SoundManagaer.Instance.MasterVolume = value;
        }

        private void SetSoundEffectsLevel(float value)
        {
            //Plays a jump sound effect to check the volume of the sfx
            if (!_soundEffectPlaying)
                StartCoroutine(PlayJumpSound());
            SoundManagaer.Instance.Mixer.SetFloat("SoundEffectsVolume", Mathf.Log10(value) * 20);
            SoundManagaer.Instance.SoundEffectsVolume = value;
        }
        //Plays a jump sound effect to check the volume of the sfx and wait some time
        //after being able to display another.
        IEnumerator PlayJumpSound()
        {
            _soundEffectPlaying = true;
            SoundManagaer.Instance.PlayJump();
            yield return new WaitForSecondsRealtime(0.5f);
            _soundEffectPlaying = false;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
