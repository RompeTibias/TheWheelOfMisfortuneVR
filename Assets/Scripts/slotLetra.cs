using TMPro;
using UnityEngine;

public class slotLetra : MonoBehaviour
{
    [SerializeField] private TextMeshPro letra;
    [SerializeField] private GameObject caraDelante;
    [SerializeField] private GameObject caraDetras;
    public char letraActual;
    public bool oculta; // true cuando la letra está oculta, false cuando se ha revelado o es espacio

    public void SetLetra(char nuevaLetra)
    {
        letraActual = nuevaLetra;

        if (nuevaLetra == ' ')
        {
            // Caso de espacio vacío en la palabra
            oculta = false;
            caraDelante.SetActive(false);
            caraDetras.SetActive(false);
            letra.text = "";
        }
        else
        {
            // Caso de casilla con letra oculta
            oculta = true;

            // Mostramos la cara posterior (reverso/oculta) y ocultamos la delantera
            caraDelante.SetActive(false);
            caraDetras.SetActive(true);

            // Dejamos el texto vacío para que no se vea la letra
            letra.text = "";
        }
    }

    public void Revelar()
    {
        // Revelamos la letra
        oculta = false;

        // Muestra la cara frontal y oculta el reverso
        caraDetras.SetActive(false);
        caraDelante.SetActive(true);

        // Asignamos la letra al TextMeshPro para que aparezca
        letra.text = letraActual.ToString();
    }
}
