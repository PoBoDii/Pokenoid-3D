using UnityEngine;

public class BolasExtras : MonoBehaviour, IPowerUp
{
    public int numeroBolas = 2;

    public void Activar(GameObject bumper)
    {
        GameObject[] bolas = GameObject.FindGameObjectsWithTag("Bola");

        GameUIManager gameUI = FindAnyObjectByType<GameUIManager>();

        gameUI.MostrarPowerup("2 Pokeballs Extra");
        AudioManager.instance.SoundOtrosPowerups();

        foreach (GameObject bola in bolas)
        {
            PowerUpBola script = bola.GetComponent<PowerUpBola>();
            controlBola control = bola.GetComponent<controlBola>();
            
            if (script != null)
            {
                
                script.bolasExtra(control.direccion);
            }
        }
    }
}
