using UnityEngine;

public class enganchaBola : MonoBehaviour
{
    public GameObject prefabBola;
    [HideInInspector] public bool inicioPartida = true;
    private GameObject bola;

    void Awake()
    {
        iniciaBola();
    }

    void Update()
    {
        if(inicioPartida)
        {
            bola.transform.position = transform.position + new Vector3(0f, 0f, 0.45f);
        }

        if (Input.GetKey(KeyCode.Space) && inicioPartida)
        {
            bool desactivaControl = FindAnyObjectByType<controlBumper>().desactivaInicio;
            if(!desactivaControl)
            {
                inicioPartida = false;
                controlBola controlBola = bola.GetComponent<controlBola>();
                controlBola.Inicializar(new Vector3(-0.5f, 0f, 1f));
            }
        }
    }

    public void iniciaBola()
    {
        bola = Instantiate(prefabBola, transform.position + new Vector3(0f, 0f, 1.5f), Quaternion.identity);
        //Aseguramos que empezamos con Pokeball
        Transform poke = bola.transform.Find("PokeBall/PokeBall");
        Transform quick = bola.transform.Find("QuickBall/QuickBall");
        if (poke != null) poke.gameObject.SetActive(true);
        if (quick != null) quick.gameObject.SetActive(false);
        inicioPartida = true;
    }
}
