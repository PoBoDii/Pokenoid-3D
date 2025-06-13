using UnityEngine;

public class Normalball : MonoBehaviour, IPowerUp
{
    public void Activar(GameObject bumper)
    {
        GameObject[] bolas = GameObject.FindGameObjectsWithTag("Bola");

        GameUIManager gameUI = FindAnyObjectByType<GameUIManager>();

        gameUI.MostrarPowerup("Desactivando Powerball");
        AudioManager.instance.SoundOtrosPowerups();

        foreach (GameObject bola in bolas)
        {
            PowerUpBola script = bola.GetComponent<PowerUpBola>();

            if (script != null)
            {
                
                // Activa Quickball y desactiva Pokeball
                Transform poke = bola.transform.Find("PokeBall/PokeBall");
                Transform quick = bola.transform.Find("QuickBall/QuickBall");

                if (poke != null) poke.gameObject.SetActive(true);
                if (quick != null) quick.gameObject.SetActive(false);

                script.desactivaPowerBall();
            }
        }
    }
}