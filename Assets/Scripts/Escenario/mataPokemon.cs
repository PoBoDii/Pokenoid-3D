using System.Collections;
using UnityEngine;

public class mataPokemon : MonoBehaviour
{
    public int puntos = 500;
    public GameObject captureEffectPrefab;
    public Material materialBlancoPrefab;

    private Vector3 posicionInicial;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bola"))
        {
            GameUIManager manager = FindAnyObjectByType<GameUIManager>();
            manager.AddScore(puntos);

            GameObject GestorLadrillos = GameObject.FindGameObjectWithTag("GestorLadrillos");
            LadrilloManager script = GestorLadrillos.GetComponent<LadrilloManager>();
            script.LadrilloDestruido();
            AudioManager.instance.BreakPokemonSound();

            CapturaPokemon();
        }
    }

    public void CapturaPokemon()
    {
        CambiarColorBlanco();
        DesactivarColisiones();

        // Guarda posición inicial para mantener el centro al escalar
        posicionInicial = transform.position;

        // Instancia efecto de captura
        GameObject captureEffect = Instantiate(captureEffectPrefab, transform.position, Quaternion.identity);
        ParticleSystem ps = captureEffect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(captureEffect, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(captureEffect, 2f);
        }

        // Inicia animación de escala y elevación
        StartCoroutine(AnimacionCaptura());
    }

    IEnumerator AnimacionCaptura()
    {
        float duracion = 2.0f;
        float altura = 1.0f; // altura hacia arriba al desaparecer
        Vector3 escalaInicial = transform.localScale;
        Vector3 escalaFinal = Vector3.zero;
        Vector3 posicionFinal = posicionInicial + new Vector3(0, altura, 0);
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float t = tiempo / duracion;
            transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);
            transform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.localScale = escalaFinal;
        Destroy(gameObject);
    }

    public void CambiarColorBlanco()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_Color"))
            {
                rend.material.color = Color.white;
            }
            else
            {
                rend.material = materialBlancoPrefab;
            }
        }
    }

    public void DesactivarColisiones()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }
}
