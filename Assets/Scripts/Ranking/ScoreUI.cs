using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_Text scoreText;
    public ScoreManager scoreManager; // referencia al ScoreManager

    void Start()
    {
        int score = ScoreData.lastScore;
        scoreText.text = $"{score}";
    }

    public void SaveScore()
    {
        string playerName = nameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            scoreManager.AddScore(playerName, ScoreData.lastScore);
            Debug.Log("Puntuación guardada correctamente");
            SceneManager.LoadScene("HighScores");
        }
        else
        {
            Debug.LogWarning("Debes introducir un nombre");
        }
    }
}
