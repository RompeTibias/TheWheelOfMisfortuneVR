 using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class managerPanel : MonoBehaviour
{
    // ORDEN L>R las letras del panel de letras 
    //no las hago un aidrectamente por que creo que para el tema de salto de lineas puede ser que las necesitemos separadas
    [SerializeField] private List<GameObject> fila1 = new List<GameObject>();
    [SerializeField] private List<GameObject> fila2 = new List<GameObject>();
    [SerializeField] private List<GameObject> fila3 = new List<GameObject>();
    [SerializeField] private List<GameObject> fila4 = new List<GameObject>();
    //[SerializeField] private List<GameObject> fila5 = new List<GameObject>();
    [SerializeField] private string frasePrueba = "Github si no paras de autocompletarme las lineas te voy a arrancar las tripas;";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CargarFrase(frasePrueba);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Aciertos(char letraIntentada)//cuando se elija la letra hay que llamar a etse metodo y como parametro la letra 
    {
        int aciertos = 0;
        List<GameObject> filasJuntas = new List<GameObject>(); //junta todas las listas para comprobar si la letra esta en alguna de ellas
        filasJuntas.AddRange(fila1);
        filasJuntas.AddRange(fila2);
        filasJuntas.AddRange(fila3);
        filasJuntas.AddRange(fila4);
        //filasJuntas.AddRange(fila5);

        for (int i = 0; i < filasJuntas.Count; i++) //lo de oculta revisar que soy dislexico y no se si lo he escrito bien
        {
            slotLetra slot = filasJuntas[i].GetComponent<slotLetra>();
            if (slot.letraActual == letraIntentada && slot.oculta)
            {
                slot.Revelar();
                aciertos++;

                

            }
           if (aciertos==0)
            {
                Debug.Log("la letra " + letraIntentada + " no esta en la frase");
            }

            else
            {
                Debug.Log("la letra " + letraIntentada + " esta en la frase y se ha revelado " + aciertos + " veces");
            }


        }


        return aciertos;
    }
    
    public void CargarFrase(string frase)
    {
        frase = frase.ToUpper();//fuerza todo en mayusculas asi escribimos las frases sin preocupacion
                                //creo que no deberiamos meter acentos por que creo que unity diferencia las letras con y sin acentos

        string[] lineas = { "", "", "", "" };
        int filaActual = 0;
        string[] palabras = frase.Split(" ", System.StringSplitOptions.RemoveEmptyEntries); // el coso este borra los espacios chatgpteada de epoca 
         foreach (string palabra in palabras)
        {
            if (filaActual >= 4) break;

            if (lineas[filaActual]== "")
            {
                lineas[filaActual] = palabra;
            }
            else
            {
                if (lineas[filaActual].Length + 1 + palabra.Length <=14)
                {
                    lineas[filaActual] += " " + palabra;
                }
                else
                {
                    filaActual++;
                    if (filaActual<4)
                    {
                        lineas[filaActual] = palabra;
                    }
                }
            }
        }

         List<List<GameObject>>todasLasFilas = new List<List<GameObject>> { fila1, fila2, fila3, fila4 };
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < todasLasFilas[i].Count; j++)  // github lo retiro te quiero 
            {
                slotLetra slot = todasLasFilas[i][j].GetComponent<slotLetra>();
                if (j < lineas[i].Length)
                {
                    slot.SetLetra(lineas[i][j]);
                }
                else
                {
                    slot.SetLetra(' ');
                }
            }
        }

    }
}
