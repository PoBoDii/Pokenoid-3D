using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LadrilloManager : MonoBehaviour
{
    public GameObject Medalla;
    private GameUIManager gameManager;

    private List<GameObject> ladrillos = new List<GameObject>();
    private int totalLadrillos;
    private int ladrillosRestantes;
    private int pokemonRestantes;
    private int preLadrillos;
    private bool medallaGenerada = false;
    private bool acabaNivel = false;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameUIManager>();

        acabaNivel = false;
        // Buscar todos los ladrillos al inicio
        totalLadrillos = GameObject.FindGameObjectsWithTag("Ladrillo").ToList().Count;
        pokemonRestantes = GameObject.FindGameObjectsWithTag("Pokemon").ToList().Count;
        ladrillosRestantes = totalLadrillos;
        preLadrillos = ladrillosRestantes;
    }

    public void Update()
    {
        ladrillosRestantes = GameObject.FindGameObjectsWithTag("Ladrillo").ToList().Count;
        pokemonRestantes = GameObject.FindGameObjectsWithTag("Pokemon").ToList().Count;
        if ((ladrillosRestantes + pokemonRestantes) - 1 <= 0 && !acabaNivel)
        {
            FadeController transicion = FindAnyObjectByType<FadeController>();
            transicion.FadeIn(3f);
            acabaNivel = true;
        }
    }

    public void LadrilloDestruido()
    {
        //También sumamos puntuación dependiendo del número de ladrillos rotos desde la última iteración
        gameManager.AddScore(500*(preLadrillos-ladrillosRestantes));
        preLadrillos = ladrillosRestantes;

        float porcentaje = GetPorcentajeRestante();
        if (porcentaje < 5 && !medallaGenerada)
        {
            if (!GameObject.FindGameObjectWithTag("Medalla"))
            {
                ActivaObjetoFinPartida();
                medallaGenerada = true;
            }
        }
    }

    public float GetPorcentajeRestante()
    {
        return (float)ladrillosRestantes / totalLadrillos * 100f;
    }

    private void ActivaObjetoFinPartida()
    {
        //Instanciamos la medalla en el centro del mapa
        Instantiate(Medalla, new(0f, 0.5f, 0f), Quaternion.identity);
    }
}
