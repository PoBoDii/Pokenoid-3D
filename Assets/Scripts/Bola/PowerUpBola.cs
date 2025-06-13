using UnityEngine;

public class PowerUpBola : MonoBehaviour
{
    public GameObject prefabBola;

    public void bolasExtra(Vector3 dirOriginal)
    {
        //Instanciar la primera bola rotada 60 grados respecto a la original
        GameObject newBola1 = Instantiate(prefabBola, transform.position, Quaternion.identity);
        controlBola controlNewBola1 = newBola1.GetComponent<controlBola>();

        controlBola bolaActual = GetComponent<controlBola>();

        if (controlNewBola1 != null )
        {
            Vector3 newDir1 = Quaternion.AngleAxis(120f, Vector3.up) * dirOriginal;
            controlNewBola1.Inicializar(newDir1.normalized);
            if(bolaActual.imanActivado) controlNewBola1.imanActivado = true;
        }


        //Instanciar la segunda bola rotada 120 grados respecto a la original
        GameObject newBola2 = Instantiate(prefabBola, transform.position, Quaternion.identity);
        controlBola controlNewBola2 = newBola2.GetComponent<controlBola>();

        if (controlNewBola2 != null)
        {
            Vector3 newDir2 = Quaternion.AngleAxis(-120f, Vector3.up) * dirOriginal;
            controlNewBola2.Inicializar(newDir2.normalized);
            if (bolaActual.imanActivado) controlNewBola2.imanActivado = true;
        }
    }

    //Activamos o desactivamos el PowerBall para activar o desactivar rebote con los ladrillos
    public void activaPowerBall()
    {
        controlBola ctrl = GetComponent<controlBola>();
        ctrl.PowerBall = true;
    }

    public void desactivaPowerBall()
    {
        controlBola ctrl = GetComponent<controlBola>();
        ctrl.PowerBall = false;
    }

    public void masVelocidad()
    {
        controlBola ctrl = GetComponent<controlBola>();
        ctrl.velocidad *= 1.2f;
    }

    public void menosVelocidad()
    {
        controlBola ctrl = GetComponent<controlBola>();
        ctrl.velocidad /= 1.2f;
    }
}
