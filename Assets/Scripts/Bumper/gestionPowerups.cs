using System.Drawing;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gestionPowerups : MonoBehaviour
{
    //Power-Up 1 - Expandir bumper
    //Función para agrandar el bumper temporalmente

    public controlBumper scriptControl;
    public float duracionExpansion = 0.5f;

    private Coroutine corrutinaIman;
    private bool timerRuning = false;

    private GameObject pokedexNormal;
    private GameObject pokedexIman;

    public GameUIManager gameUIManager;

    public void Start()
    {
        gameUIManager = FindAnyObjectByType<GameUIManager>();
        gameUIManager.imanPowerupUI.gameObject.SetActive(false);
        gameUIManager.imanIcon.gameObject.SetActive(false);

        pokedexNormal = transform.Find("Pokedex")?.gameObject;
        pokedexIman = transform.Find("PokedexIman")?.gameObject;

        pokedexIman.SetActive(false);

        if (pokedexNormal == null || pokedexIman == null)
        {
            Debug.LogError("No se encontraron los objetos 'Pokedex' o 'PokedexIman' como hijos del bumper.");
            return;
        }
    }

    public void ActivarIman()
    {
        pokedexNormal.SetActive(false);
        pokedexIman.SetActive(true);
    }

    public void DesactivarIman()
    {
        pokedexNormal.SetActive(true);
        pokedexIman.SetActive(false);
    }

    public void AgrandarBumper(float factor)
    {
        if(scriptControl.size < 11.25)
        {
            float sizeOriginal = scriptControl.size;
            float sizeFinal = sizeOriginal * factor;

            StartCoroutine(EscalarSuavemente(sizeOriginal, sizeFinal, duracionExpansion)); // duración del escalado suave
        }
    }

    public void ReducirBumper(float factor)
    {
        if (scriptControl.size > 2.23)
        {
            float sizeOriginal = scriptControl.size;
            float sizeFinal = sizeOriginal / factor;

            StartCoroutine(EscalarSuavemente(sizeOriginal, sizeFinal, duracionExpansion)); // duración del escalado suave
        }
    }


    //Función para escalar el bumper de manera gradual (mejor experiencia de usuario)
    private IEnumerator EscalarSuavemente(float sizeOriginal, float sizeFinal, float duracion)
    {
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            //El clamp coge 2 números y normaliza su diferencia entre 0 y 1, pa usar el lerp
            float t = Mathf.Clamp01(tiempo / duracion);
            //El lerp interpola 2 números teniendo en cuenta t (t = 0, sizeOriginal; t = 1, sizeFinal)
            scriptControl.size = Mathf.Lerp(sizeOriginal, sizeFinal, t);

            // Asegura que el bumper no se pase de los límites
            float limiteIzquierdo = -scriptControl.limiteX + scriptControl.size / 2f;
            float limiteDerecho = scriptControl.limiteX - scriptControl.size / 2f;
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
            transform.position = pos;

            yield return null;
        }
    }

    public void ActivarImanPorTiempo(float segundos)
    {
        if (timerRuning && corrutinaIman != null)
        {
            StopCoroutine(corrutinaIman); // detener la anterior si estaba activa
        }

        gameUIManager = FindAnyObjectByType<GameUIManager>();
        gameUIManager.imanPowerupUI.gameObject.SetActive(true);
        gameUIManager.imanIcon.gameObject.SetActive(true);
        corrutinaIman = StartCoroutine(TemporizadorIman(segundos)); // iniciar nueva
        timerRuning = true;
    }

    private IEnumerator TemporizadorIman(float segundos)
    {
        //Cambiamos el modelo al del iman
        ActivarIman();

        //Activamos el iman en todas las bolas
        GameObject[] bolasIni = GameObject.FindGameObjectsWithTag("Bola");
        foreach (GameObject bola in bolasIni)
        {
            controlBola script = bola.GetComponent<controlBola>();

            if (script != null)
            {
                script.imanActivado = true;
            }
        }

        //Esperamos los segundos del timer
        float tiempoRestante = segundos;
        while (tiempoRestante > 0)
        {
            gameUIManager.imanPowerupUI.text = tiempoRestante.ToString() + "s";
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }

        //Cambiamos el modelo al de la Pokedex
        DesactivarIman();
        gameUIManager.imanPowerupUI.gameObject.SetActive(false);
        gameUIManager.imanIcon.gameObject.SetActive(false);

        //Desactivamos el iman en todas las bolas
        GameObject[] bolasFin = GameObject.FindGameObjectsWithTag("Bola");
        foreach (GameObject bola in bolasFin)
        {
            controlBola script = bola.GetComponent<controlBola>();
            

            if (script != null)
            {
                script.imanActivado = false;
                //Además, lanzamos todas las bolas para que no se queden estancadas
                script.lanzarBola();
            }
        }

        timerRuning = false;
    }

    public void AcabaNivel()
    {
        Debug.Log("HAS SUPERADO EL NIVEL");

        //Actualizamos en la UI con el número del siguiente nivel (índice de la escena)
        GameObject gameUI = GameObject.FindGameObjectWithTag("GameUI");
        GameUIManager manager = gameUI.GetComponent<GameUIManager>();
        FadeController fc = FindAnyObjectByType<FadeController>();

        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelIndex <= 5)
        {
            manager.SetLevel(nextLevelIndex);
            fc.FadeAndLoadScene(nextLevelIndex);
        }
        else
        {
            ScoreData.lastScore = manager.score;
            fc.FadeAndLoadScene(6);
        }
    }
}
