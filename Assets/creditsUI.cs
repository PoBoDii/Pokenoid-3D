using UnityEngine;
using UnityEngine.SceneManagement;

public class creditsUI : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
