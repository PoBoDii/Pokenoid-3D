using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform pivotPoint;    // Punto de pivote de la c�mara
    public float rotationSpeed = 100f;   // Velocidad de rotaci�n
    public float verticalSpeed = 80f;    // Velocidad vertical
    public float distanciaCamara = -10f;

    public float minYAngle = 10f;   // L�mite inferior
    public float maxYAngle = 80f;   // L�mite superior
    public float minXAngle = -45;   // L�mite izquierda
    public float maxXAngle = 45;   // L�mite derecha

    public float zoomSpeed = 3f;           // Velocidad del zoom
    public float minZoomDistance = -15f;   // M�xima distancia (m�s alejado)
    public float maxZoomDistance = -25f;    // M�nima distancia (m�s cerca)



    [HideInInspector] public float currentX = 0f;
    [HideInInspector] public float currentY = 0f;

    public bool controlActivo = true; //Para desactivar el control al hacer animaci�n inicial

    void Start()
    {
        // Establecer la posici�n inicial de la c�mara
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (!controlActivo) return;  // No mover c�mara durante la intro

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            currentX += mouseX * rotationSpeed * Time.deltaTime;
            currentY -= mouseY * verticalSpeed * Time.deltaTime;

            // Limitar el �ngulo vertical
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
            currentX = Mathf.Clamp(currentX, minXAngle, maxXAngle);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            distanciaCamara += scrollInput * zoomSpeed;
            distanciaCamara = Mathf.Clamp(distanciaCamara, minZoomDistance, maxZoomDistance);
        }

        // Rotar la c�mara alrededor del pivote
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = pivotPoint.position + rotation * new Vector3(0, 0, distanciaCamara);
        transform.LookAt(pivotPoint);
    }

    public void AlejarCamara(float distanciaFinal, float duracion)
    {
        Debug.Log("Alejando cam");
        StartCoroutine(AlejarCorutina(distanciaFinal, duracion));
    }

    private IEnumerator AlejarCorutina(float distanciaFinal, float duracion)
    {
        controlActivo = false; // Bloquea el control del jugador

        Vector3 inicio = transform.position;
        Vector3 direccion = (transform.position - pivotPoint.position).normalized;
        Vector3 destino = pivotPoint.position + direccion * distanciaFinal;

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime; // Ignora Time.timeScale
            float t = tiempo / duracion;
            transform.position = Vector3.Lerp(inicio, destino, t);
            transform.LookAt(pivotPoint);
            yield return new WaitForEndOfFrame();
        }

        transform.position = destino;
        transform.LookAt(pivotPoint);
    }


}