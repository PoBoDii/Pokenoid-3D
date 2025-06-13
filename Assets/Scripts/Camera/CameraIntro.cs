using UnityEngine;

public class CameraIntro : MonoBehaviour
{
    public Transform cameraTransform; // Cámara
    public Transform pivotPoint;  // Punto alrededor del cual rotará la cámara
    public float duration = 5f; //Duración de la vuelta inicial
    private float rotationSpeed;

    private float elapsedTime = 0f;
    private bool isRotating = true;
    public float offsetIntro = 20f;
    private float distInicio = -30f;
    private float distCamara = -10f;

    void Start()
    {
        CameraControl cc = cameraTransform.GetComponent<CameraControl>();
        cc.controlActivo = false;
        distCamara = cc.distanciaCamara;
        distInicio = distCamara - offsetIntro;

        Quaternion rotation = Quaternion.Euler(cc.currentY, cc.currentX, 0);
        cameraTransform.position = cc.pivotPoint.position + rotation * new Vector3(0, 0, distCamara);
        cameraTransform.LookAt(cc.pivotPoint);

        // Calculamos la vuelta como los grados que se tiene que mover cada segundo para completar la rotación entera
        rotationSpeed = 360f / duration;
    }

    void Update()
    {
        if (isRotating)
        {
            elapsedTime += Time.deltaTime;

            // Calcular distancia interpolada
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            float currentDistance = Mathf.Lerp(distInicio, distCamara, t);

            cameraTransform.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
            Vector3 dir = (cameraTransform.position - pivotPoint.position).normalized;
            cameraTransform.position = pivotPoint.position + dir * -currentDistance;

            if (elapsedTime >= duration)
            {
                isRotating = false;

                // Activar el control de cámara después de la vuelta
                cameraTransform.GetComponent<CameraControl>().controlActivo = true;
            }
        }
    }

    public void StartRotation()
    {
        elapsedTime = 0f;
        isRotating = true;
    }
}
