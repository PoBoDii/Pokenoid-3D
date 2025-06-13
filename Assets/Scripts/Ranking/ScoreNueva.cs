using UnityEngine;
using UnityEngine.UI;

public class ScoreNueva : MonoBehaviour
{
    public InputField nameInput;
    public int currentScore;
    public ScoreManager scoreManager;

    public void Submit()
    {
        string playerName = nameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            scoreManager.AddScore(playerName, currentScore);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ranking");
        }
    }
}
