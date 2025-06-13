using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioMixer mixer;

    public AudioSource musicSource;

    public AudioClip mainMenu;
    public AudioClip level1;
    public AudioClip level2;
    public AudioClip level3;
    public AudioClip level4;
    public AudioClip level5;
    public AudioClip gameOver;

    private string currentSceneName = "";
    private AudioClip currentClip = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f); // 0.7 es el valor por defecto si no hay nada guardado
        SetMusicVolume(savedVolume);
    }

    private void Update()
    {
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene != currentSceneName)
        {
            currentSceneName = activeScene;
            UpdateMusicForScene(activeScene);
        }
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    void UpdateMusicForScene(string sceneName)
    {
        AudioClip newClip = null;

        switch (sceneName)
        {
            case "MainMenu":
            case "Instructions":
            case "Options":
            case "HighScores":
                newClip = mainMenu;
                break;
            case "Level1":
                newClip = level1;
                break;
            case "Level2":
                newClip = level2;
                break;
            case "Level3":
                newClip = level3;
                break;
            case "Level4":
                newClip = level4;
                break;
            case "Level5":
                newClip = level5;
                break;
            case "GameOver":
                newClip = gameOver;
                break;
        }

        if (newClip != null && newClip != currentClip)
        {
            musicSource.clip = newClip;
            musicSource.loop = true;
            musicSource.Play();
            currentClip = newClip;
        }
    }

}
