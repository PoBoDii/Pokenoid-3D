using UnityEngine;

public class SumaVida : MonoBehaviour, IPowerUp
{
    public void Activar(GameObject bumper)
    {
        GameUIManager gameManager = FindAnyObjectByType<GameUIManager>();

        GameUIManager gameUI = FindAnyObjectByType<GameUIManager>();

        gameUI.MostrarPowerup("1 Vida Extra!");
        AudioManager.instance.SoundVidaPowerup();

        if (gameManager != null)
        {
            gameManager.GainLife();
        }
    }
}