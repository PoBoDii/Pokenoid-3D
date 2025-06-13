using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.Collections;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    public GameObject powerupMessagePrefab;
    public Transform powerupMessagePanel;
    public int maxPowerupMessages = 4;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;

    private enganchaBola bumper;

    public CameraIntro cameraIntro;

    public TextMeshProUGUI imanPowerupUI;
    public Image imanIcon;

    public int score = 0;
    public int lives = 3;
    public int level = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        UpdateScore();
        UpdateLives();

        level = SceneManager.GetActiveScene().buildIndex;
        UpdateLevel();

        // Iniciar la rotación de la cámara al inicio
        if (cameraIntro != null)
        {
            cameraIntro.StartRotation();
        }
    }

    private void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Si no es una escena de juego, salimos
        if (!sceneName.StartsWith("Level")) return;

        if (GameObject.FindGameObjectsWithTag("Bola").Length == 0)
        {
            LoseLife();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetLevel(1);
            SceneManager.LoadScene("Level1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetLevel(2);
            SceneManager.LoadScene("Level2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetLevel(3);
            SceneManager.LoadScene("Level3");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetLevel(4);
            SceneManager.LoadScene("Level4");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetLevel(5);
            SceneManager.LoadScene("Level5");
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    public void LoseLife()
    {
        lives--;
        UpdateLives();

        if (lives <= 0)
        {
            UnityEngine.Debug.Log("Game Over");
            ScoreData.lastScore = score;
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            //Borra todos los powerups que estén en el mapa
            GameObject[] capsulas = GameObject.FindGameObjectsWithTag("Capsula");
            foreach(GameObject c in capsulas)
            {
                Destroy(c);
            }

            // Reincia la bola en el bumper
            bumper = FindAnyObjectByType<enganchaBola>();
            bumper.iniciaBola();
        }
    }

    public void GainLife()
    {
        lives++;
        UpdateLives();
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        UpdateLevel();

        // Iniciar la rotación de la cámara al cambiar de nivel
        if (cameraIntro != null)
        {
            cameraIntro.StartRotation();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = "SCORE " + score;
    }

    private void UpdateLives()
    {
        livesText.text = "LIVES " + lives;
    }

    private void UpdateLevel()
    {
        levelText.text = "LEVEL " + level;
    }

    public void MostrarPowerup(string mensaje)
    {
        // Eliminar el mensaje más antiguo si hay demasiados
        if (powerupMessagePanel.childCount >= maxPowerupMessages)
        {
            Transform mensajeAntiguo = powerupMessagePanel.GetChild(0);
            CanvasGroup cgAntiguo = mensajeAntiguo.GetComponent<CanvasGroup>();
            StopCoroutine(DesvanecerMensaje(cgAntiguo, 3f)); // por si acaso, cancelamos si ya hay una en curso
            StartCoroutine(DesvanecerMensaje(cgAntiguo, 0.5f)); // desvanecemos rápidamente
        }

        // Instanciar y configurar el nuevo mensaje
        GameObject nuevoMensaje = Instantiate(powerupMessagePrefab, powerupMessagePanel);
        TextMeshProUGUI texto = nuevoMensaje.GetComponent<TextMeshProUGUI>();
        texto.text = mensaje;
        nuevoMensaje.SetActive(true);

        CanvasGroup cg = nuevoMensaje.GetComponent<CanvasGroup>();
        StartCoroutine(DesvanecerMensaje(cg, 3f));
    }


    private IEnumerator DesvanecerMensaje(CanvasGroup cg, float duracion)
    {
        if (cg == null) yield break;

        float tiempo = 0f;
        float alphaInicial = cg.alpha;

        while (tiempo < duracion)
        {
            if (cg == null) yield break; // prevenir si se destruyó

            tiempo += Time.deltaTime;
            cg.alpha = Mathf.Lerp(alphaInicial, 0f, tiempo / duracion);
            yield return null;
        }

        if (cg != null)
            Destroy(cg.gameObject);
    }
}
