using UnityEngine;

public class Iman : MonoBehaviour, IPowerUp
{
    public float segundos = 10f;
    public void Activar(GameObject bumper)
    {
        
        GameUIManager gameUI = GameObject.FindAnyObjectByType<GameUIManager>();

        gameUI.MostrarPowerup("Iman Activado (15s)");
        AudioManager.instance.SoundOtrosPowerups();

        gestionPowerups script = bumper.GetComponent<gestionPowerups>();
        script.ActivarImanPorTiempo(segundos);
    }
}