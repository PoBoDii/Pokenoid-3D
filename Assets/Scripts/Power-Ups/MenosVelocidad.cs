using UnityEngine;

public class MenosVelocidad : MonoBehaviour, IPowerUp
{
    public void Activar(GameObject bumper)
    {
        GameObject[] bolas = GameObject.FindGameObjectsWithTag("Bola");

        GameUIManager gameUI = FindAnyObjectByType<GameUIManager>();

        gameUI.MostrarPowerup("- Velocidad Pokeballs");
        AudioManager.instance.SoundOtrosPowerups();

        foreach (GameObject bola in bolas)
        {
            PowerUpBola script = bola.GetComponent<PowerUpBola>();

            if (script != null)
            {
                script.menosVelocidad();
            }
        }
    }
}
