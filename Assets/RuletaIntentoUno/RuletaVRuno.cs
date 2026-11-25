using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RuletaVRuno : MonoBehaviour
{
    [SerializeField] private Transform centroRuleta;
    [SerializeField] private Vector3 ejeRotacionLocal = Vector3.forward;
    [SerializeField] private float friccion = 0.5f;
    [SerializeField] private float suavizadoMando = 15f;

    [Header("Quesitos")]
    [SerializeField, Min(1)] private int cantidadQuesitos = 8;
    [SerializeField] private bool numerosEnSentidoPositivo = true;
    [SerializeField] private float desfaseInicialGrados;

    public float BloqueoGiro;
    public bool EstaGirando { get; private set; }
    public int QuesitoCaido { get; private set; } = -1;
    private bool resultadoDisponible;

    public bool IntentarTomarResultado(out int indice)
    {
        indice = QuesitoCaido;
        if (EstaGirando || !resultadoDisponible)
            return false;

        resultadoDisponible = false;
        return true;
    }

    private const float VelocidadMinima = 0.01f;
    private XRBaseInteractable[] interactables;
    private IXRSelectInteractor interactorActual;
    private IXRSelectInteractable interactableActual;
    private Vector3 direccionPreviaMundo;
    private float velocidadAngular;
    private float anguloAcumulado;
    private bool resultadoPendiente;

    private void Awake()
    {
        if (centroRuleta == null)
            centroRuleta = transform;

        interactables = GetComponentsInChildren<XRBaseInteractable>(true);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        foreach (var interactable in interactables)
        {
            interactable.selectEntered.AddListener(AlAgarrar);
            interactable.selectExited.AddListener(AlSoltar);
        }
    }

    private void OnDisable()
    {
        foreach (var interactable in interactables)
        {
            interactable.selectEntered.RemoveListener(AlAgarrar);
            interactable.selectExited.RemoveListener(AlSoltar);
        }

        SoltarAgarre();
        velocidadAngular = 0f;
        EstaGirando = false;
        RegistrarResultado();
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        if (BloqueoGiro > 0f || interactorActual != null)
            return;

        interactorActual = args.interactorObject;
        interactableActual = args.interactableObject;

        Vector3 ejeMundo = centroRuleta.TransformDirection(ejeRotacionLocal);
        direccionPreviaMundo = ObtenerDireccionMando(ejeMundo);
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        if (args.interactorObject == interactorActual &&
            args.interactableObject == interactableActual)
        {
            SoltarAgarre();
        }
    }

    private void SoltarAgarre()
    {
        interactorActual = null;
        interactableActual = null;
    }

    private void Update()
    {
        if (BloqueoGiro > 0f && interactorActual != null)
            SoltarAgarre();

        EstaGirando = false;
        Vector3 ejeMundo = centroRuleta.TransformDirection(ejeRotacionLocal);
        float tiempo = Time.deltaTime;

        if (interactorActual != null)
        {
            Vector3 direccionActual = ObtenerDireccionMando(ejeMundo);
            float deltaAngulo = 0f;

            if (direccionPreviaMundo.sqrMagnitude > 0.001f &&
                direccionActual.sqrMagnitude > 0.001f)
            {
                deltaAngulo = Vector3.SignedAngle(
                    direccionPreviaMundo, direccionActual, ejeMundo);

                Girar(ejeMundo, deltaAngulo);
            }

            direccionPreviaMundo = direccionActual;

            if (tiempo > 0f)
            {
                velocidadAngular = Mathf.Lerp(
                    velocidadAngular, deltaAngulo / tiempo, tiempo * suavizadoMando);
            }

            return;
        }

        if (Mathf.Abs(velocidadAngular) > VelocidadMinima)
        {
            Girar(ejeMundo, velocidadAngular * tiempo);

            float resistencia = Mathf.Max(0.001f, friccion);
            float frenado = (Mathf.Abs(velocidadAngular) * 0.5f + 10f)
                * resistencia * tiempo;

            velocidadAngular = Mathf.MoveTowards(velocidadAngular, 0f, frenado);
        }

        if (Mathf.Abs(velocidadAngular) <= VelocidadMinima)
        {
            velocidadAngular = 0f;
            EstaGirando = false;
            RegistrarResultado();
        }
    }

    private Vector3 ObtenerDireccionMando(Vector3 ejeMundo)
    {
        Vector3 haciaMando = interactorActual.transform.position - centroRuleta.position;
        return Vector3.ProjectOnPlane(haciaMando, ejeMundo).normalized;
    }

    private void Girar(Vector3 ejeMundo, float grados)
    {
        if (grados == 0f)
            return;

        centroRuleta.Rotate(ejeMundo, grados, Space.World);
        anguloAcumulado = Mathf.Repeat(anguloAcumulado + grados, 360f);
        EstaGirando = true;
        resultadoPendiente = true;
        QuesitoCaido = -1;
        resultadoDisponible = false;
    }

    private void RegistrarResultado()
    {
        if (!resultadoPendiente)
            return;

        int total = Mathf.Max(1, cantidadQuesitos);
        float gradosPorQuesito = 360f / total;
        float sentido = numerosEnSentidoPositivo ? 1f : -1f;
        float anguloBajoPuntero = desfaseInicialGrados - anguloAcumulado * sentido;
        float angulo = Mathf.Repeat(anguloBajoPuntero + gradosPorQuesito * 0.5f, 360f);

        QuesitoCaido = Mathf.FloorToInt(angulo / gradosPorQuesito) % total;
        resultadoDisponible = true;
        resultadoPendiente = false;
    }
}
