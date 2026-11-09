using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Management;

public class HeadsetInfo : MonoBehaviour
{
    [SerializeField] XROrigin origin;
    [SerializeField] TMP_Text label;

    void Update()
    {
        // Detectar el runtime XR activo
        XRLoader loader = XRGeneralSettings.Instance.Manager.activeLoader;

        // Nombre del runtime
        string runtime = "Simulador";

        if (loader != null)
        {
            runtime = loader.name;
        }

        // Altura de la cámara sobre el suelo
        float height = origin.CameraInOriginSpaceHeight;

        // Giro de la cabeza alrededor del eje vertical
        float yaw = origin.Camera.transform.eulerAngles.y;

        // Escribir los datos en el panel
        label.text = $"Runtime: {runtime}\n" +
                     $"Altura de los ojos: {height:0.00} m\n" +
                     $"Giro: {yaw:0}°";
    }
}