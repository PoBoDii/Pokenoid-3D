using UnityEngine;

public class controlCapsula : MonoBehaviour
{
    private GameUIManager gameManager;
    public float velocidadCaida = 2f;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameUIManager>();
    }

    void Update()
    {
        transform.position += Time.deltaTime * velocidadCaida * new Vector3(0, 0, -1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bumper"))
        {
            IPowerUp powerUp = GetComponent<IPowerUp>();
            if (powerUp != null)
            {
                // Activar el power-up
                powerUp.Activar(other.gameObject);
            }

            gameManager.AddScore(3500);
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.z < -15)
        {
            Destroy(gameObject); // Se ha perdido
        }
    }
}
