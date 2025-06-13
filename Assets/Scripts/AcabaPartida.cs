using UnityEngine;
using UnityEngine.SceneManagement;

public class AcabaPartida : MonoBehaviour, IPowerUp
{
    public void Activar(GameObject bumper)
    {
        FadeController transicion = FindAnyObjectByType<FadeController>();
        transicion.FadeIn(3f);
    }
}
