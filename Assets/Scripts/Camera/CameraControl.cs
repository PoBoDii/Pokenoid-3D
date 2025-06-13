using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform pivotPoint;    // Punto de pivote de la cámara
    public float rotationSpeed = 100f;   // Velocidad de rotación
    public float verticalSpeed = 80f;    // Velocidad vertical
    public float distanciaCamara = -10f;

    public float minYAngle = 10f;   // Límite inferior
    public float maxYAngle = 80f;   // Límite superior
    public float minXAngle = -45;   // Límite izquierda
    public float maxXAngle = 45;   // Límite derecha

    public float zoomSpeed = 3f;           // Velocidad del zoom
    public float minZoomDistance = -15f;   // Máxima distancia (más alejado)
    public float maxZoomDistance = -25f;    // Mínima distancia (más cerca)



    [HideInInspector] public float currentX = 0f;
    [HideInInspector] public float currentY = 0f;

    public bool controlActivo = true; //Para desactivar el control al hacer animación inicial

    void Start()
    {
        // Establecer la posición inicial de la cámara
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (!controlActivo) return;  // No mover cámara durante la intro

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            currentX += mouseX * rotationSpeed * Time.deltaTime;
            currentY -= mouseY * verticalSpeed * Time.deltaTime;

            // Limitar el ángulo vertical
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
            currentX = Mathf.Clamp(currentX, minXAngle, maxXAngle);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            distanciaCamara += scrollInput * zoomSpeed;
            distanciaCamara = Mathf.Clamp(distanciaCamara, minZoomDistance, maxZoomDistance);
        }

        // Rotar la cámara alrededor del pivote
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