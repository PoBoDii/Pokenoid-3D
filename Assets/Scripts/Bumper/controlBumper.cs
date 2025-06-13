using System.Collections;
using UnityEngine;

public class controlBumper : MonoBehaviour
{
    public float velocidad = 10f;
    public float limiteX = 6f; // Límite horizontal (ajusta según tu escena)
    public float size = 4f;
    public GameObject muroGodmode;

    private float inicioTimer = 0f;
    [HideInInspector] public bool desactivaInicio = true;

    private void Start()
    {
        muroGodmode.SetActive(false);
        inicioTimer = FindAnyObjectByType<CameraIntro>().duration;
        desactivaInicio = true;
        StartCoroutine(timerInicio(inicioTimer));
        FindAnyObjectByType<GodmodeController>().SetIconVisible(false);
    }

    void Update()
    {
        //Actualizamos la anchura del bumper
        Vector3 escalaBumper = transform.localScale;
        transform.localScale = new Vector3(size, escalaBumper.y, escalaBumper.z);

        float movimiento = 0f;

        //Calculamos los limites teniendo en cuenta la medida actual del bumper
        float limiteIzq = -limiteX + size / 2;
        float limiteDer = limiteX - size / 2;


        // Controles por teclado
        if(!desactivaInicio)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movimiento = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                movimiento = 1f;
            }
        }

        if (movimiento != 0f)
        {
            Vector3 nuevaPos = transform.position + movimiento * Time.deltaTime * velocidad * Vector3.right;
            //Cercionamos que no nos salimos de los limites
            nuevaPos.x = Mathf.Clamp(nuevaPos.x, limiteIzq, limiteDer);
            transform.position = nuevaPos;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!muroGodmode.activeSelf)
            {
                muroGodmode.SetActive(true);
                Debug.Log("GODMODE ACTIVADO");
            }
            else
            {
                muroGodmode.SetActive(false);
                Debug.Log("GODMODE DESACTIVADO");
            }
        }
    }

    private IEnumerator timerInicio(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        desactivaInicio = false;
    }
}
