using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsManager : MonoBehaviour
{
    public GameObject[] panels; // Array con los 5 paneles
    private int currentPanel = 0;

    void Start()
    {
        ShowPanel(currentPanel);
    }

    // Mostrar el panel en la posici�n 'index' y ocultar los dem�s
    void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }

    // Ir al siguiente panel (si no es el �ltimo)
    public void NextPanel()
    {
        if (currentPanel < panels.Length - 1)
        {
            currentPanel++;
            ShowPanel(currentPanel);
        }
    }

    // Ir al panel anterior (si no es el primero)
    public void PreviousPanel()
    {
        if (currentPanel > 0)
        {
            currentPanel--;
            ShowPanel(currentPanel);
        }
    }

    // Volver al men� principal
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}


