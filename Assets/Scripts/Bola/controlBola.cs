using System.Collections;
using UnityEngine;

public class controlBola : MonoBehaviour
{
    public float velocidad = 5f;
    public Vector3 direccion;

    public bool imanActivado = false;
    private bool sigueBumper = false;
    private Vector3 offsetPegado;
    private Transform bumperPegado;


    //Esta variable sirve para evitar que la bola rompa 2 bloques a la vez (si choca justo en el centro)
    private bool AlreadyCollided = false;
    



    //Se edita en el script de PowerUpBola
    [HideInInspector] public bool PowerBall = false;

    public void Inicializar(Vector3 dir)
    {
        direccion = dir;
    }

    public void Start()
    {
        AlreadyCollided = false;
    }

    void FixedUpdate()
    {
        AlreadyCollided = false;
    }

    void Update()
    {
        //Hacemos que dejen de colisionar para evitar que las que llegan nuevas choquen con las ya pegadas al bumper
        if (imanActivado)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bola"), LayerMask.NameToLayer("Bola"), true);
        }
        else
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bola"), LayerMask.NameToLayer("Bola"), false);
        }

        //Si está pegado al bumper, esperamos a darle a espacio para lanzar las bolas
        if (sigueBumper && bumperPegado != null)
        {
            transform.position = bumperPegado.position + offsetPegado;

            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                bool desactivaControl = FindAnyObjectByType<controlBumper>().desactivaInicio;
                if (!desactivaControl) lanzarBola();
            }
        }
        else
        {
            Vector3 movimiento = Time.deltaTime * velocidad * direccion;
            transform.position += movimiento;

            // ROTACIÓN DE LA BOLA
            if (direccion != Vector3.zero)
            {
                float radio = 0.5f; // ajusta esto al tamaño real de tu bola (scale Y / 2 por lo general)
                float distancia = movimiento.magnitude;
                float angulo = distancia / radio * Mathf.Rad2Deg;

                // Eje de rotación = dirección cruzada con Vector3.up (para simular rodar)
                Vector3 ejeRotacion = Vector3.Cross(direccion.normalized, Vector3.up);

                transform.Rotate(ejeRotacion, angulo, Space.World);
            }
        }

        if (transform.position.z < -15) Destroy(gameObject);

        // Corregir dirección casi horizontal
        float minZ = 0.3f; // umbral mínimo de inclinación en Z

        if (Mathf.Abs(direccion.z) < minZ)
        {
            float sign = Mathf.Sign(direccion.z) == 0 ? 1f : Mathf.Sign(direccion.z);
            direccion.z = sign * minZ;

            // Recalcula la dirección para que siga normalizada
            direccion = direccion.normalized;
        }

    }


    void OnCollisionEnter(Collision collision)
    {

        // Rebote horizontal (muros superior/inferior)
        if (collision.gameObject.CompareTag("MuroHorizontal"))
        {
            direccion.x *= -1;
        }
        else if (collision.gameObject.CompareTag("MuroVertical") || collision.gameObject.CompareTag("MuroGodMode"))
        {
            direccion.z *= -1;
        }
        // Rebote con el bumper
        else if (collision.gameObject.CompareTag("Bumper"))
        {
            if(!imanActivado)
            {
                if(!FindAnyObjectByType<enganchaBola>().inicioPartida) AudioManager.instance.SoundPaleta();
                GameObject bumper = collision.gameObject;
                Vector3 punto = collision.GetContact(0).point;

                float anchoBumper = bumper.transform.localScale.x;
                float centroBumper = bumper.transform.position.x;

                // Para calcular la dirección a la que rebota la bola, restamos la posición de esta con la del bumper
                // y esto lo dividimos entre el ancho del bumper / 2 para normalizarlo (el entre 2 es porque quedará un número positivo o negativo dependiendo
                // de si la bola pega más a la izquierda o derecha del medio, así que seria como si el bumper realmente tuvera 2 lados de colisión
                float offsetX = (punto.x - centroBumper) / (anchoBumper / 2f);
                // Una vez calculado, por si acaso hacemos un clamp para que de ningún modo se vaya hacia abajo
                offsetX = Mathf.Clamp(offsetX, -1f, 1f);

                // Aplicamos la dirección X calculada anteriormente, y la Z la ponemos a 1 para que vaya hacia arriba
                direccion = new Vector3(offsetX, 0f, 1f).normalized;
            }
            else
            {
                ContactPoint contacto = collision.GetContact(0);
                offsetPegado = contacto.point - collision.transform.position; // vector desde el centro del bumper al punto de colisión
                offsetPegado.z += 0.25f;
                bumperPegado = collision.transform;

                sigueBumper = true;
                direccion = Vector3.zero; // se queda quieta
            }
        }
        //Colisión con objetos ladrillos
        else if(collision.gameObject.CompareTag("Ladrillo") || collision.gameObject.CompareTag("Pokemon"))
        {
            if (PowerBall)
            {
                if (collision.gameObject.CompareTag("Ladrillo")) collision.gameObject.GetComponent<controlLadrillo>().DestruyeLadrillo();
                return; //Si está en PowerBall, no rebota con el ladrillo
            }

            if (AlreadyCollided) return; //Evita que la bola choque con 2 ladrillo si pilla justo en medio
            AlreadyCollided = true;

            ContactPoint contacto = collision.GetContact(0);
            Vector3 normal = contacto.normal;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
            {
                direccion.x *= -1; // rebote horizontal
            }
            else
            {
                direccion.z *= -1; // rebote vertical
            }

            if(collision.gameObject.CompareTag("Ladrillo")) collision.gameObject.GetComponent<controlLadrillo>().DestruyeLadrillo();
        }
    }

    public void lanzarBola() {
        if(sigueBumper)
        {
            float lado = 0f;
            if (offsetPegado.x >= 0)
            {
                lado = 0.5f;
            }
            else lado = -0.5f;

            direccion = new Vector3(lado, 0f, 1f).normalized;

            sigueBumper = false;
            bumperPegado = null;
        }
    }
}


