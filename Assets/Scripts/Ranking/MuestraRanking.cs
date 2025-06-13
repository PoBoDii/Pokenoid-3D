using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuestraRanking : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform container;
    public ScoreManager scoreManager;

    void Start()
    {
        MostrarRanking();
    }

    public void MostrarRanking()
    {
        foreach (Transform child in container)
        {
            if (child.gameObject.CompareTag("Entry")) // Solo borra los entries
                Destroy(child.gameObject);
        }

        var scores = scoreManager.scoreList.scores;

        // Ordenar de mayor a menor puntuación
        scores.Sort((a, b) => b.score.CompareTo(a.score));

        // Definir el gradiente de colores: dorado (top) a gris (último)
        Color topColor = new Color(1f, 0.84f, 0f); // Dorado
        Color lowColor = Color.gray;

        for (int i = 0; i < scores.Count; i++)
        {
            var entry = scores[i];
            GameObject go = Instantiate(entryPrefab, container);
            go.tag = "Entry";

            // Calcular color del gradiente
            float t = scores.Count == 1 ? 0f : (float)i / (scores.Count - 1);
            Color backgroundColor = Color.Lerp(topColor, lowColor, t);

            // Setear datos del jugador
            go.transform.Find("NombreTexto").GetComponent<TextMeshProUGUI>().text = entry.playerName;
            go.transform.Find("ScoreTexto").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();

            // Aplicar color al fondo (Image del contenedor del prefab)
            Image backgroundImage = go.GetComponent<Image>();
            if (backgroundImage != null)
            {
                backgroundImage.color = backgroundColor;
            }
            else
            {
                // Si el Image está en un hijo llamado "Background"
                var bg = go.transform.Find("Background");
                if (bg != null && bg.GetComponent<Image>() != null)
                {
                    bg.GetComponent<Image>().color = backgroundColor;
                }
                else
                {
                    Debug.LogWarning("No se encontró el Image para el fondo en el prefab.");
                }
            }
        }
    }



    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
