using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class managerPanel : MonoBehaviour
{
    internal bool LetrasGestionadasPorTurno { get; set; }
    public static event Action<int> AciertosEvent;

    // ORDEN L>R las letras del panel de letras 
    // protected para que los hijos puedan leer les listas
    [SerializeField] protected List<GameObject> fila1 = new List<GameObject>();
    [SerializeField] protected List<GameObject> fila2 = new List<GameObject>();
    [SerializeField] protected List<GameObject> fila3 = new List<GameObject>();
    [SerializeField] protected List<GameObject> fila4 = new List<GameObject>();
    [SerializeField] protected List<GameObject> fila5 = new List<GameObject>();

    [SerializeField] protected string frasePrueba = "Github si no paras de autocompletarme las lineas te voy a arrancar las tripas;";

    // virtual permite a los hijosn añadir
    protected virtual void OnEnable()
    {
        LetrasElegir.OnLetraElegida += ProcesarLetraRecibida;
    }

    protected virtual void OnDisable()
    {
        LetrasElegir.OnLetraElegida -= ProcesarLetraRecibida;
    }

    // lo mismo el virtual para que los hijos puedan modificar
    protected virtual void ProcesarLetraRecibida(char letra)
    {
        if (!LetrasGestionadasPorTurno)
            Aciertos(letra);
    }

    protected virtual void Start()
    {
        CargarFrase(frasePrueba);
    }

    protected virtual void Update()
    {

    }

    
    public virtual void Aciertos(char letraIntentada)
    {
        int aciertos = 0;
        List<GameObject> filasJuntas = new List<GameObject>();
        filasJuntas.AddRange(fila1);
        filasJuntas.AddRange(fila2);
        filasJuntas.AddRange(fila3);
        filasJuntas.AddRange(fila4);
        filasJuntas.AddRange(fila5);

        for (int i = 0; i < filasJuntas.Count; i++)
        {
            slotLetra slot = filasJuntas[i].GetComponent<slotLetra>();
            if (slot.letraActual == letraIntentada && slot.oculta)
            {
                slot.Revelar();
                aciertos++;
            }
        }

       
        if (aciertos == 0)
        {
            Debug.Log("la letra " + letraIntentada + " no esta en la frase");
        }
        else
        {
            Debug.Log("la letra " + letraIntentada + " esta en la frase y se ha revelado " + aciertos + " veces");
        }

        AciertosEvent?.Invoke(aciertos);
        //return aciertos;
    }

    public virtual void CargarFrase(string frase)
    {
        frase = frase.ToUpper();

        List<List<GameObject>> todasLasFilas = new List<List<GameObject>> { fila1, fila2, fila3, fila4, fila5 };
        string[] lineas = new string[todasLasFilas.Count];
        for (int i = 0; i < lineas.Length; i++)
        {
            lineas[i] = string.Empty;
        }
        int filaActual = 0;
        string[] palabras = frase.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string palabra in palabras)
        {
            if (filaActual >= lineas.Length) break;

            if (lineas[filaActual].Length == 0)
            {
                lineas[filaActual] = palabra;
            }
            else if (lineas[filaActual].Length + 1 + palabra.Length <= 14)
            {
                lineas[filaActual] += " " + palabra;
            }
            else
            {
                filaActual++;
                if (filaActual < lineas.Length)
                {
                    lineas[filaActual] = palabra;
                }
            }
        }

        for (int i = 0; i < lineas.Length; i++)
        {
            int espaciosDisponibles = todasLasFilas[i].Count - lineas[i].Length;
            int margenIzquierdo = Mathf.Max(0, (espaciosDisponibles + 1) / 2);

            for (int j = 0; j < todasLasFilas[i].Count; j++)
            {
                slotLetra slot = todasLasFilas[i][j].GetComponent<slotLetra>();
                int posicionEnLaFila = j - margenIzquierdo;
                if (posicionEnLaFila >= 0 && posicionEnLaFila < lineas[i].Length)
                {
                    slot.SetLetra(lineas[i][posicionEnLaFila]);
                }
                else
                {
                    slot.SetLetra(' ');
                }
            }
        }
    }
}
