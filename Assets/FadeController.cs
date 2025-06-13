using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Arrastra aquí el Image del FadePanel desde el inspector

    void Start()
    {
        // Empieza invisible
        SetAlpha(0f);
    }

    public void FadeIn(float duration = 2f)
    {
        Time.timeScale = 0f;

        CameraControl cc = FindAnyObjectByType<CameraControl>();
        cc.AlejarCamara(50f, duration); // aleja la cámara a 40 unidades del pivote

        StartCoroutine(FadeTo(1f, duration - 0.5f)); // fade algo más corto que el zoom
    }

    public void FadeOut(float duration = 1.5f)
    {
        StartCoroutine(FadeTo(0f, duration)); // fade algo más corto que el zoom
    }

    public void FadeAndLoadScene(int sceneIndex, float duration = 2f)
    {
        StartCoroutine(FadeAndSwitchScene(sceneIndex, duration));
    }

    public void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }

    private IEnumerator FadeAndSwitchScene(int sceneIndex, float duration)
    {
        yield return FadeToCoroutine(1f, duration); // fade out
        SceneManager.LoadScene(sceneIndex);
        yield return new WaitForEndOfFrame(); // aseguramos que la escena cargó
        SetAlpha(1f); // nos aseguramos de que siga en negro
        Time.timeScale = 1f;
        FindAnyObjectByType<GodmodeController>().SetIconVisible(false);
        yield return FadeToCoroutine(0f, duration); // fade in
    }
    private IEnumerator FadeToCoroutine(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            SetAlpha(newAlpha);
            yield return new WaitForEndOfFrame();
        }

        SetAlpha(targetAlpha);
    }


    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // usamos tiempo real
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            SetAlpha(newAlpha);
            yield return new WaitForEndOfFrame();
        }

        SetAlpha(targetAlpha);

        // Llamamos a la función que pasa al siguiente nivel
        gestionPowerups bumperPowerups = GameObject.FindAnyObjectByType<gestionPowerups>();
        if (bumperPowerups != null)
            bumperPowerups.AcabaNivel();
    }
}
