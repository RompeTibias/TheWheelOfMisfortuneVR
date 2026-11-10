using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class slotLetra : MonoBehaviour
{
    [SerializeField] private TextMeshPro letra;
    [SerializeField] private GameObject caraDelante;
    [SerializeField] private GameObject caraDetras;
    public char letraActual;
    public bool oculta;//esto se puede usar para edetectar los espacios vacios en el manager futuro
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLetra(char nuevaLetra)
    {
        letraActual = nuevaLetra;
        if (nuevaLetra == ' ')
        {
            //si añadimos skin para el espacio cambiar el setactive de la cara d
            oculta = false;
            caraDelante.SetActive(false);
            caraDetras.SetActive(false);
            letra.text = ""; //limpiar la anterior
        }
        else
        {
            oculta = true ;
           //cambair al contrario las dos de abajo cuando este lo de revelar
            caraDelante.SetActive(true);
            caraDetras.SetActive(false);
            letra.text = letraActual.ToString();//desactivar esto cuando este la logica de revelar funcionando 
           
        }
    }
    public void Revelar()
    {
        oculta = false;
        caraDetras.SetActive(false);
        caraDelante.SetActive(true);
        letra.text = letraActual.ToString();//el textmesh solo coje strings no chars es lo que hay
    }
}
