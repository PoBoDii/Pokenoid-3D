using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mixer;

    public AudioSource sfxSource;

    public AudioClip blockBreakSound;
    public AudioClip pokemonBreakSound;
    public AudioClip VidaPowerupSound;
    public AudioClip OtrosPowerupsSound;
    public AudioClip PaletaSound;


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
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f); // 0.7 es el valor por defecto si no hay nada guardado
        SetSFXVolume(savedVolume);
    }

    public void BreakBlockSound()
    {
        sfxSource.PlayOneShot(blockBreakSound);
    }
    public void BreakPokemonSound()
    {
        sfxSource.PlayOneShot(pokemonBreakSound);
    }
    public void SoundVidaPowerup()
    {
        sfxSource.PlayOneShot(VidaPowerupSound);
    }
    public void SoundOtrosPowerups()
    {
        sfxSource.PlayOneShot(OtrosPowerupsSound);
    }
    public void SoundPaleta()
    {
        sfxSource.PlayOneShot(PaletaSound);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
