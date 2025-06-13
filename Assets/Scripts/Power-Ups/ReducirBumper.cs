using UnityEngine;

public class ReducirBumper : MonoBehaviour, IPowerUp
{
    public float factorEscala = 1.5f;

    public void Activar(GameObject bumper)
    {
        gestionPowerups powerUps = bumper.GetComponent<gestionPowerups>();

        GameUIManager gameUI = FindAnyObjectByType<GameUIManager>();

        gameUI.MostrarPowerup("- Tamaño Pokedex");
        AudioManager.instance.SoundOtrosPowerups();

        if (powerUps != null)
        {
            powerUps.ReducirBumper(factorEscala);
        }
    }
}