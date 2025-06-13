using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // M�todo para iniciar el juego
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // M�todo para abrir Instrucciones
    public void OpenInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    // M�todo para abrir Opciones
    public void OpenOptions()
    {
        SceneManager.LoadScene("Options");
    }

    // M�todo para ver el High Score
    public void OpenHighScore()
    {
        SceneManager.LoadScene("HighScores");
    }

    // M�todo para salir del juego
    public void QuitGame()
    {
        Debug.Log("Juego cerrado");
        UnityEngine.Application.Quit();
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
