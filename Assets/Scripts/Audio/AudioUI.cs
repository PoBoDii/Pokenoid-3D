using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Cargar valores desde PlayerPrefs o usar por defecto 0.7
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        Debug.Log("Valor slider " + value + ", dBs " + Mathf.Log10(value) * 20 + " prefs " + PlayerPrefs.GetFloat("MusicVolume", 0.7f));
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
        
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
