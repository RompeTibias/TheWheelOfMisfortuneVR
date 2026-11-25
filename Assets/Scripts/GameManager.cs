using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float pointsActual;
    public float pointsNeeded;
    public float actualRound;
    public float moneyActual;
    private int hitsActual;
    [System.Serializable]
    private class Quesito
    {
        public QuesitoRuleta ficha;
        public float puntos = 1f;
        public GameObject indicador;
        public Renderer rendererSector;
        [Min(0)] public int indiceMaterial;
    }

    [SerializeField] private RuletaVRuno ruletaVRuno;
    [SerializeField] private managerPanel panel;
    [SerializeField] private Quesito[] quesitos = new Quesito[0];

    private int quesitoPendiente = -1;
    private GameObject indicadorActivo;

    private void Awake()
    {
        if (ruletaVRuno == null)
            ruletaVRuno = FindFirstObjectByType<RuletaVRuno>();
        if (panel == null)
            panel = GetComponent<managerPanel>();
        if (panel == null)
            panel = FindFirstObjectByType<managerPanel>();

        foreach (Quesito quesito in quesitos)
        {
            if (quesito?.indicador != null)
                quesito.indicador.SetActive(false);

            if (quesito?.ficha?.material == null || quesito.rendererSector == null)
                continue;

            Material[] materiales = quesito.rendererSector.sharedMaterials;
            if (quesito.indiceMaterial < 0 || quesito.indiceMaterial >= materiales.Length)
                continue;

            materiales[quesito.indiceMaterial] = quesito.ficha.material;
            quesito.rendererSector.sharedMaterials = materiales;
        }
    }

    private void OnEnable()
    {
        managerPanel.AciertosEvent += HitsEvent;
        LetrasElegir.OnLetraElegida += ProcesarLetra;
       
        if (ruletaVRuno == null || panel == null)

        {
            Debug.LogError("GameManager necesita una ruleta y un panel de letras.", this);
            return;
        }

        panel.LetrasGestionadasPorTurno = true;
       

    }


    private void OnDisable()
    {

        managerPanel.AciertosEvent -= HitsEvent;
        LetrasElegir.OnLetraElegida -= ProcesarLetra;


        if (panel != null)
            panel.LetrasGestionadasPorTurno = false;
        if (quesitoPendiente >= 0 && ruletaVRuno != null)
            ruletaVRuno.BloqueoGiro = 0f;
        quesitoPendiente = -1;
    }

    private void Update()
    {
        if (quesitoPendiente >= 0 || ruletaVRuno == null || panel == null ||
            !ruletaVRuno.IntentarTomarResultado(out int indice))
            return;

        quesitoPendiente = indice;
        ruletaVRuno.BloqueoGiro = 1f;

        if (indicadorActivo != null)
            indicadorActivo.SetActive(false);
        indicadorActivo = null;

        EjecutarHabilidad(ObtenerQuesito(indice)?.ficha?.postGiro);
    }

    private void ProcesarLetra(char letra)
    {
        if (quesitoPendiente < 0)
            return;

        int indice = quesitoPendiente;
        quesitoPendiente = -1;
        letra = char.ToUpperInvariant(letra);

        try
        {
            Quesito quesito = ObtenerQuesito(indice);
            QuesitoRuleta ficha = quesito?.ficha;
            if (hitsActual == 0)
            {
                EjecutarHabilidad(ficha?.fallo, letra);
                return;
            }

            pointsActual += (quesito?.puntos ?? 1f) * hitsActual;
            indicadorActivo = quesito?.indicador;
            if (indicadorActivo != null)
                indicadorActivo.SetActive(true);
            EjecutarHabilidad(ficha?.acierto, letra, hitsActual);
        }
        finally
        {
            ruletaVRuno.BloqueoGiro = 0f;
        }
    }

    // Llama a este método al confirmar que la frase se ha resuelto.
    public void ConfirmarResolucion()
    {
        if (quesitoPendiente < 0)
            return;

        QuesitoRuleta ficha = ObtenerQuesito(quesitoPendiente)?.ficha;
        quesitoPendiente = -1;
        try
        {
            EjecutarHabilidad(ficha?.resolver);
        }
        finally
        {
            ruletaVRuno.BloqueoGiro = 0f;
        }
    }

    private void EjecutarHabilidad(HabilidadQuesito habilidad, char? letra = null, int aciertos = 0)
    {
        if (habilidad != null)
            habilidad.Ejecutar(this, letra, aciertos);
    }

    private Quesito ObtenerQuesito(int indice)
    {
        return indice >= 0 && indice < quesitos.Length ? quesitos[indice] : null;
    }
    private void HitsEvent(int aciertos)
    {
        hitsActual = aciertos;
    }
}

