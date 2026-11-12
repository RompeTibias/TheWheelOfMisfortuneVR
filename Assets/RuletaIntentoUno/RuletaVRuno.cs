
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Control de ruleta estilo "Ruleta de la Suerte" en VR.
/// Mantiene la inercia del impulso al soltar sin cambios bruscos de fuerza.
/// </summary>
public class RuletaVR : MonoBehaviour
{
    [Header("Referencias Principales")]

    [Tooltip("El pivote o centro que rotará (si se deja vacío, usa este objeto).")]
    [SerializeField] private Transform centroRuleta;

    [Header("Configuración de Eje")]

    [Tooltip("Eje local de giro: (0,0,1) para pared, (0,1,0) para mesa.")]
    [SerializeField] private Vector3 ejeRotacionLocal = Vector3.forward;

    [Header("Física de la Ruleta de la Suerte")]

    [Tooltip("Resistencia al giro. Valores bajos (0.3 - 0.8) hacen que gire durante más tiempo.")]
    [SerializeField] private float friccion = 0.5f;

    [Tooltip("Filtro de suavizado de la mano. Evita tirones o saltos bruscos justo al soltar.")]
    [SerializeField] private float suavizadoMando = 15f;

    private XRBaseInteractable[] interactables;
    private IXRSelectInteractor interactorActual;

    private Vector3 direccionPreviaMundo;
    private float velocidadAngular = 0f;
    private bool estaAgarrado = false;

    private void Awake()
    {
        if (centroRuleta == null)
            centroRuleta = transform;

        interactables = GetComponentsInChildren<XRBaseInteractable>(true);

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        if (interactables != null)
        {
            foreach (var inter in interactables)
            {
                inter.selectEntered.AddListener(AlAgarrar);
                inter.selectExited.AddListener(AlSoltar);
            }
        }
    }

    private void OnDisable()
    {
        if (interactables != null)
        {
            foreach (var inter in interactables)
            {
                inter.selectEntered.RemoveListener(AlAgarrar);
                inter.selectExited.RemoveListener(AlSoltar);
            }
        }
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        interactorActual = args.interactorObject;
        estaAgarrado = true;

        Vector3 ejeMundo = centroRuleta.TransformDirection(ejeRotacionLocal);

        Vector3 vecInter = interactorActual.transform.position - centroRuleta.position;

        direccionPreviaMundo = Vector3.ProjectOnPlane(vecInter, ejeMundo).normalized;
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        interactorActual = null;
        estaAgarrado = false;

        // Al soltar, el valor acumulado en velocidadAngular
        // se mantiene intacto sin tirones.
    }

    private void Update()
    {
        Vector3 ejeMundo = centroRuleta.TransformDirection(ejeRotacionLocal);

        if (estaAgarrado && interactorActual != null)
        {
            Vector3 vecActual = interactorActual.transform.position - centroRuleta.position;

            Vector3 direccionActualMundo = Vector3.ProjectOnPlane(vecActual, ejeMundo).normalized;

            if (direccionPreviaMundo.sqrMagnitude > 0.001f && direccionActualMundo.sqrMagnitude > 0.001f)
            {
                float deltaAngulo = Vector3.SignedAngle(
                    direccionPreviaMundo,
                    direccionActualMundo,
                    ejeMundo
                );

                // Rotar la ruleta siguiendo el movimiento del mando
                centroRuleta.Rotate(ejeMundo, deltaAngulo, Space.World);

                // Calcular velocidad instantánea y filtrarla progresivamente
                if (Time.deltaTime > 0f)
                {
                    float velInstantanea = deltaAngulo / Time.deltaTime;

                    velocidadAngular = Mathf.Lerp(
                        velocidadAngular,
                        velInstantanea,
                        Time.deltaTime * suavizadoMando
                    );
                }

                direccionPreviaMundo = direccionActualMundo;
            }
        }
        else if (Mathf.Abs(velocidadAngular) > 0.01f)
        {
            // La ruleta sigue girando tras soltarla con la velocidad acumulada
            float rotacionFrame = velocidadAngular * Time.deltaTime;

            centroRuleta.Rotate(ejeMundo, rotacionFrame, Space.World);

            // Frenado progresivo natural (resistencia proporcional + fricción constante)
            float desaceleracion = (
                Mathf.Abs(velocidadAngular) * friccion * 0.5f
                + 10f * friccion
            ) * Time.deltaTime;

            velocidadAngular = Mathf.MoveTowards(
                velocidadAngular,
                0f,
                desaceleracion
            );
        }
    }
}
